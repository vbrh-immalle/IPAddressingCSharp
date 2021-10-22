using System;
using System.Net;

namespace IPAddressing
{
    class Program
    {
        static void Main(string[] args)
        {
            TrySomeSimpleDataStructures();
            Console.WriteLine();
            TryDotnetsIPAddressClass();
            Console.WriteLine();
            TrySomeSubnetCalculations();
            Console.WriteLine();
            TrySomeSubnetCalculations("192.168.0.1", 24);
            Console.WriteLine();
            TrySomeSubnetCalculations("192.168.1.1", 24);
            Console.WriteLine();
        }

        static void TrySomeSimpleDataStructures()
        {
            Console.WriteLine("Some experiments with simple data structures to store IP addresses");
            // An IPv4 address has 4 bytes.

            // Theoretically we could use an unsigned int to store it.

            // We use hexadecimal notation to store the "ip address" 255.255.255.255.
            // (Actually this is a broadcast address and not a real IP address.)
            uint broadcast_address_uint = 0xFFFFFFFF;

            // Or we convert (manually) every byte to hexadecimal notation:
            // 192 = 0xC0 (hex) = 1100 0000 (bin)
            // 168 = 0xA8 (hex) = 1010 1000 (bin)
            // 0 = 0x00 = 0000 0000
            // 1 = 0x1 (hex) = 0000 0001
            uint ip_address_uint = 0xC0A80001;

            // We could also store it in an array of 4 unsigned bytes
            byte[] ip_address_bytes = new byte[4];

            ip_address_bytes[0] = 192;
            ip_address_bytes[1] = 168;
            ip_address_bytes[2] = 0;
            ip_address_bytes[3] = 1;

            // let's write (and use) methods that displays an ip-address-representation nicely
            PrintIPUInt(broadcast_address_uint);
            PrintIPUInt(ip_address_uint);
            PrintIPBytes(ip_address_bytes);

            // It would be handy to have a few methods to parse ip addresses and store them as uint's or byte's
            uint addr1_uint1 = CreateAddressUInt("127.0.0.1");
            uint addr1_uint2 = CreateAddressUInt(127, 0, 0, 1);
            byte[] addr1_bytes1 = CreateAddressBytes("127.0.0.1");
            byte[] addr1_bytes2 = CreateAddressBytes(127, 0, 0, 1);

            PrintIPUInt(addr1_uint1);
            PrintIPUInt(addr1_uint2);
            PrintIPBytes(addr1_bytes1);
            PrintIPBytes(addr1_bytes2);
        }

        static void TryDotnetsIPAddressClass()
        {
            // But guess what! DotNet has it's own IPAddress class!
            Console.WriteLine("System.Net.IPAddress experiments");

            // Also see: https://docs.microsoft.com/nl-nl/dotnet/api/system.net.ipaddress?view=net-5.0
            // Or check https://docs.microsoft.com/nl-nl/dotnet/api/ and look for IPAddress

            // One of the constructors takes a byte[]
            byte[] addr1_bytes1 = CreateAddressBytes("127.0.0.1"); // let's use our own function to create some bytes ...
            IPAddress addr1 = new IPAddress(addr1_bytes1); // ... and pass them to IPAddress's constructor

            // There is also a static method that can parse a string!
            // (No need for our own method anymore...)
            IPAddress addr2 = IPAddress.Parse("192.0.0.1");

            // Now we can check what properties and methods IPAddress-objects have:
            Console.WriteLine(addr1); // uses .ToString()-method
            Console.WriteLine(addr1.AddressFamily);
            byte[] addr1_bytes = addr1.GetAddressBytes();

            Console.WriteLine(IPAddress.IsLoopback(addr1)); // 127.0.0.1 is a loopback ip
            Console.WriteLine(IPAddress.IsLoopback(addr2)); // 192.0.0.1 is not a loopback ip

            // Use code completion (or browse reference documentation) to check the other:
            // - object properties and methods
            // - static methods
    }

    // change the default arguments or call with other values!
    static void TrySomeSubnetCalculations(string ipString = "12.13.14.15", int nrOfNetmaskBits = 24)
    {
        Console.WriteLine($"Calculating info about {ipString} with {nrOfNetmaskBits} netmask-bits.");
        uint ip = CreateAddressUInt(ipString);

        // let's try some subnet calculations
        // (compare with http://jodies.de/ipcalc or other ip calculators)

        uint mask = 0x80000000; // (0x80 00 00 00) = (1000 0000 0000 0000 0000 0000 0000 0000) --> we start with 1 bit ...

        // .. and loop until the variable `mask` is filled with enough bits:
        for (var i = 1; i < nrOfNetmaskBits; i++)
        {
            mask = mask >> 1; // shift a bit
            mask += 0x80000000; // add a bit
        }

        // when nrOfNetmaskBits = 24 ---> mask is now (1111 1111 1111 1111 1111 1111 0000 0000) = 240

        int nrOfNets = (int)Math.Pow(2, nrOfNetmaskBits); // TODO: should ignore 0.x.y.z
        uint net = ip & mask;
        uint nrOfHosts = (uint)(Math.Pow(2, 32 - nrOfNetmaskBits) - 2); // - 2, because 1 for broadcast, 1 for network
        Console.Write("Ip address: "); PrintIPUInt(ip);
        Console.Write("Netmask: "); PrintIPUInt(mask);
        Console.WriteLine("  -->");
        Console.WriteLine("  nr of Nets: " + nrOfNets);
        Console.WriteLine("  nr of hosts: " + nrOfHosts);
        Console.Write("  Network: "); PrintIPUInt(net);
        Console.Write("  First host: "); PrintIPUInt(net + 1);
        Console.Write("  Last host: "); PrintIPUInt(net + nrOfHosts);
        uint thisIpHostNr = ~mask & ip;
        uint thisIpNetNr = (mask & ip) >> (32 - nrOfNetmaskBits); // TODO
        Console.WriteLine("  The IP is the {0}th (0x{0:X}) host of the {1}th (0x{1:X}) net.", thisIpHostNr, thisIpNetNr);
    }



    // Printing functions:
    // -------------------


    static void PrintIPUInt(uint ip)
    {
        // >> shifts bytes x bits to the right, adding zeroes to the left
        uint a1 = ip >> 24;

        uint a2 = ip;
        a2 = a2 & 0x00FF0000;
        a2 = a2 >> 16;

        uint a3 = ip;
        a3 = a3 & 0x0000FF00;
        a3 = a3 >> 8;

        uint a4 = ip;
        a4 = a4 & 0x000000FF;

        Console.WriteLine("{0}.{1}.{2}.{3}", a1, a2, a3, a4);
    }

    static void PrintIPBytes(byte[] ip)
    {
        // we assume element 0 is the Most Significant Byte (MSB)
        Console.WriteLine("{0}.{1}.{2}.{3}", ip[0], ip[1], ip[2], ip[3]);
    }



    // Creation functions:
    // -------------------


    // Will (most likely) throw exceptions if not a correct string!
    static uint CreateAddressUInt(string ipstring)
    {
        byte[] bytes = new byte[4];
        string[] strings = new string[4];

        strings = ipstring.Split('.'); // throws exceptions if no . found?
        for (var i = 0; i < 4; i++)
        {
            bytes[i] = Convert.ToByte(strings[i]); // throws exception if number doesn't fit in a byte?
        }

        return CreateAddressUInt(bytes[0], bytes[1], bytes[2], bytes[3]);
    }

    static uint CreateAddressUInt(byte a1, byte a2, byte a3, byte a4)
    {
        uint result = 0;

        result += a1;
        result = result << 8;
        result += a2;
        result = result << 8;
        result += a3;
        result = result << 8;
        result += a4;

        return result;
    }

    static byte[] CreateAddressBytes(string ipstring)
    {
        byte[] bytes = new byte[4];
        string[] strings = new string[4];

        strings = ipstring.Split('.'); // throws exceptions if no . found?
        for (var i = 0; i < 4; i++)
        {
            bytes[i] = Convert.ToByte(strings[i]); // throws exception if number doesn't fit in a byte?
        }

        return bytes;
    }

    static byte[] CreateAddressBytes(byte a1, byte a2, byte a3, byte a4)
    {
        byte[] bytes = new byte[4];

        bytes[0] = a1;
        bytes[1] = a2;
        bytes[2] = a3;
        bytes[3] = a4;

        return bytes;
    }
}
}
