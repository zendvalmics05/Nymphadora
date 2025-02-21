namespace Nymphadora.BEncoding;

public static class BEncodeDecoder
{
    public static BEncodedElement DecodeDictionary(IEnumerator<byte> data)
    {
        data.MoveNext();
        return Decode(data);
    }
    public static BEncodedElement Decode(IEnumerator<byte> data)
    {
        if (data.Current == 'd')
        {
            var dictionary = new BEncodedDictionary();
            dictionary.Decode(data);
            return dictionary;
        }

        if (data.Current == 'l')
        {
            var list = new BEncodedList();
            list.Decode(data);
            return list;
        }
        if (data.Current >= '0' && data.Current <= '9')
        {
            var str = new BEncodedString();
            str.Decode(data);
            return str;
        }
        if (data.Current != 'i') throw new FormatException("Could not parse the current element.");
        var num = new BEncodedNumber();
        num.Decode(data);
        return num;
    }
}