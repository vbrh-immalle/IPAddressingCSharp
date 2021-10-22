# IPAdressingCSharp

This repository demonstrates:

- some experiments with different data structures to store IP addresses (`uint`, `byte[]`, `string`)
    - there is some *low-level byte-mangling* going on for the real nerds ;-)
- shows how to use the dotnet `IPAddress`-class : `System.Net.IPAddress`
    - this makes (I-want-to-code-something-with-IP-addresses-)life a bit easier
- some experiments with subnet-calculation (compare with e.g. http://jodies.de/ipcalc)

# HOWTO run

With Visual Studio 2019:

- `cd` to your projects dir
- `git clone https://github.com/vbrh-immalle/IPAddressingCSharp`
- `cd IPAddressingCSharp`
- Open `.sln`-file in Visual Studio 2019

From the command line (with `dotnet`-CLI-tools installed):

- `cd` to your projects dir
- `git clone https://github.com/vbrh-immalle/IPAddressingCSharp`
- `cd IPAddressingCSharp`
- `dotnet run`
