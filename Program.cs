﻿using Nymphadora.Core;

namespace Nymphadora;

internal static class Program
{
    public static void Main(string[] args)
    {
        
        const string s = "d8:announce33:http://192.168.1.74:6969/announce7:comment17:Comment goes here10:created by25:Transmission/2.92 (14714)13:creation datei1460444420e8:encoding5:UTF-84:infod6:lengthi59616e4:name9:lorem.txt12:piece lengthi32768e6:pieces40:L@fR���3�K*Ez�>_YS��86��\"�&�p�<�6�C{�9Ga7:privatei0eee";
        List<byte> arr = System.Text.Encoding.UTF8.GetBytes(s).ToList();
        var encoder = new BEncodeDecoder(arr.ToArray());
        encoder.PrintData();
    }
}