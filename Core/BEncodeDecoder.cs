namespace Nymphadora.Core;

public class BEncodeDecoder
{
    private readonly List<KeyValuePair<List<byte>, List<byte>>> _data = [];
    private readonly List<KeyValuePair<List<byte>, BEncodeDecoder>> _bEncode = [];
    private readonly List<List<byte>> _lists = [];

    private readonly IEnumerator<byte> _byteEnumerator;

    public BEncodeDecoder(byte[] data)
    {
        _byteEnumerator = ((IEnumerable<byte>)data).GetEnumerator();
        DecodeDictionary();
    }

    public BEncodeDecoder(IEnumerator<byte> byteEnumerator)
    {
        _byteEnumerator = byteEnumerator;
        DecodeDictionary();
    }

    private void DecodeDictionary()
    {
        try
        {
            int _ = _byteEnumerator.Current;
        }
        catch (Exception e)
        {
            _byteEnumerator.MoveNext();
        }
        if (_byteEnumerator.Current != 'd') throw new FormatException("Dictionary must start with 'd'.");
        _byteEnumerator.MoveNext();
        while (_byteEnumerator.Current != 'e')
        {
            DecodePair();
        }

        _byteEnumerator.MoveNext();
    }

    private void DecodePair()
    {
        var key = DecodeString();

        if (_byteEnumerator.Current == 'd')
        {
            var value = new BEncodeDecoder(_byteEnumerator);
            _bEncode.Add(new KeyValuePair<List<byte>, BEncodeDecoder>(key,value));
        } 
        else if (_byteEnumerator.Current == 'i')
        {
            var value = DecodeNumber();
            _data.Add(new KeyValuePair<List<byte>, List<byte>>(key,value));
        }
        else if (_byteEnumerator.Current >= '0' && _byteEnumerator.Current <= '9')
        {

            var value = DecodeString();
            _data.Add(new KeyValuePair<List<byte>, List<byte>>(key,value));
        }
        else if (_byteEnumerator.Current == 'l')
        {
            _byteEnumerator.MoveNext();
            DecodeList();
        }
        
    }

    private List<byte> DecodeNumber()
    {
        List<byte> value = [(byte)'i'];
        while (_byteEnumerator.MoveNext())
        {
            if (_byteEnumerator.Current == 'e') break;
            value.Add(_byteEnumerator.Current);
        }
        _byteEnumerator.MoveNext();
        value.Add((byte)'e');
        return value;
    }

    private List<byte> DecodeString()
    {
        List<byte> value = [];
        List<char> chars = [];
        do
        {
            value.Add(_byteEnumerator.Current);
            _byteEnumerator.MoveNext();
        } while (_byteEnumerator.Current != ':');
        var valueLengthAsString = System.Text.Encoding.UTF8.GetString(value.ToArray());
        var valueLength = Convert.ToInt32(valueLengthAsString);

        value.Add((byte)':');
        for (var i = 0; i < valueLength; i++)
        {
            _byteEnumerator.MoveNext();
            if (_byteEnumerator.Current == 239)
            {
                value.Add(_byteEnumerator.Current);
                _byteEnumerator.MoveNext();
                value.Add(_byteEnumerator.Current);
                _byteEnumerator.MoveNext();
            }
            value.Add(_byteEnumerator.Current);
        }
        
        _byteEnumerator.MoveNext();
        return value;

    }

    private void DecodeList()
    {
        if (_byteEnumerator.Current == 'i')
        {
            var value = DecodeNumber();
            _lists.Add(value);
        }
        else if (_byteEnumerator.Current >= '0' && _byteEnumerator.Current <= '9')
        {

            var value = DecodeString();
            _lists.Add(value);
        }
    }

    public void PrintData()
    {
        foreach (var pair in _data)
        {
            var key = pair.Key.Aggregate("", (current, b) => current + (char)b);
            var value = pair.Value.Aggregate("", (current, b) => current + (char)b);
            Console.WriteLine($"{key}->{value}");
        }

        foreach (var pair in _bEncode)
        {
            var key = pair.Key.Aggregate("", (current, b) => current + (char)b);
            Console.WriteLine(key + "-> {");
            pair.Value.PrintData();
            Console.WriteLine("}");
        }
    }
}