using System.Text;
using Nymphadora.BEncoding;

namespace Nymphadora;

internal static class Program
{
    public static void Main(string[] args)
    {
        var bytes = File.ReadAllBytes(
            "C:\\Users\\sagni\\Downloads\\annas_archive_meta__aacid__zlib3_files__20230808T051503Z--20250119T215605Z.jsonl.seekable.zst.torrent");
        IList<byte> encodedData = new List<byte>();
        foreach (var b in bytes)
        {
            encodedData.Add(b);
        }
        BEncodeDecoder.DecodeDictionary(encodedData.GetEnumerator());
    }
}