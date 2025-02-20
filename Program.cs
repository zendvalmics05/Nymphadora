using System.Text;
using Nymphadora.BEncoding;

namespace Nymphadora;

internal static class Program
{
    public static void Main(string[] args)
    {
        var x = new BEncodedNumber(34);
        foreach (var b in x.Encode())
        {
            Console.Write((char)b);
        }
        Console.WriteLine("Hello World!");
    }
}