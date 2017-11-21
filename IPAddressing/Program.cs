using System;

namespace IPAddressing
{
    class Program
    {
        static void Main(string[] args)
        {
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


            // But guess what? DotNet already contains an IPAddress class...
            Console.WriteLine("System.Net.IPAddress experiments");

            // Also see: https://docs.microsoft.com/nl-nl/dotnet/api/system.net.ipaddress?view=netcore-2.0
            // Or check https://docs.microsoft.com/nl-nl/dotnet/api/ and look for IPAddress

            // One of the constructors takes a byte[]
            System.Net.IPAddress addr1 = new System.Net.IPAddress(addr1_bytes1);

            // There is also a static method that can parse a string!
            System.Net.IPAddress addr2 = System.Net.IPAddress.Parse("192.0.0.1");

            // Now we can check what properties and methods IPAddress-objects have:
            Console.WriteLine(addr1); // uses .ToString()-method
            Console.WriteLine(addr1.AddressFamily);
            byte[] addr1_bytes = addr1.GetAddressBytes();

            Console.WriteLine(System.Net.IPAddress.IsLoopback(addr1)); // 127.0.0.1 is a loopback ip
            Console.WriteLine(System.Net.IPAddress.IsLoopback(addr2)); // 192.0.0.1 is not a loopback ip
            
            

        }

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

        // Will (most likely) throw exceptions if not a correct string!
        static uint CreateAddressUInt(string ipstring)
        {
            byte[] bytes = new byte[4];
            string[] strings = new string[4];

            strings = ipstring.Split('.'); // throws exceptions if no . found?
            for(var i = 0; i < 4; i++)
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
