using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Nymphadora.BEncoding;

public class BEncodedDictionary(Dictionary<BEncodedString, BEncodedElement> elements)
    : BEncodedElement, IDictionary<BEncodedString, BEncodedElement>
{
    private Dictionary<BEncodedString, BEncodedElement> Elements { get; set; } = elements;

    public BEncodedDictionary() : this(new Dictionary<BEncodedString, BEncodedElement>())
    {
    }

    public override int Length() => 2 + Elements.Sum(element => element.Key.Length() + element.Value.Length());

    public override void Decode(IEnumerator<byte> data)
    {
        if (data.Current != 'd')
        {
            throw new FormatException("BEncodedDictionary must start with 'd'.");
        }
        var dictionary = new BEncodedDictionary();

        while (data.MoveNext())
        {
            Console.WriteLine("Parsing dictionary elements: " + (char)data.Current);
            if (data.Current == 'e')
            {
                Elements = dictionary.Elements;
                return;
            }
            var key = new BEncodedString();
            key.Decode(data);
            data.MoveNext();
            var value = BEncodeDecoder.Decode(data);
            dictionary.Add(key, value);
        }
    }

    public override byte[] Encode()
    {
        var arr = new byte[Length()];
        arr[0] = (byte)'d';
        var idx = 1;

        foreach (var element in Elements)
        {
            var keyCpy = element.Key.Encode();
            foreach (var b in keyCpy)
            {
                arr[idx++] = b;
            }
            var valueCpy = element.Value.Encode();
            foreach (var b in valueCpy)
            {
                arr[idx++] = b;
            }
        }
        arr[idx] = (byte)'e';
        return arr;
    }

    public IEnumerator<KeyValuePair<BEncodedString, BEncodedElement>> GetEnumerator()
    {
        return Elements.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(KeyValuePair<BEncodedString, BEncodedElement> item)
    {
        Elements.Add(item.Key, item.Value);
    }

    public void Clear()
    {
        Elements.Clear();
    }

    public bool Contains(KeyValuePair<BEncodedString, BEncodedElement> item)
    {
        return Elements.Contains(item);
    }

    public void CopyTo(KeyValuePair<BEncodedString, BEncodedElement>[] array, int arrayIndex)
    {
        foreach (var element in Elements)
        {
            array[arrayIndex++] = new KeyValuePair<BEncodedString, BEncodedElement>(element.Key, element.Value);
        }
    }

    public bool Remove(KeyValuePair<BEncodedString, BEncodedElement> item)
    {
        return Elements.Remove(item.Key);
    }

    public int Count => Elements.Count;
    public bool IsReadOnly => false;
    public void Add(BEncodedString key, BEncodedElement value)
    {
        Elements.Add(key, value);
    }

    public bool ContainsKey(BEncodedString key)
    {
        return Elements.ContainsKey(key);
    }

    public bool Remove(BEncodedString key)
    {
        return Elements.Remove(key);
    }

    public bool TryGetValue(BEncodedString key, [MaybeNullWhen(false)] out BEncodedElement value)
    {
        return Elements.TryGetValue(key, out value);
    }

    public BEncodedElement this[BEncodedString key]
    {
        get => Elements[key];
        set => Elements[key] = value;
    }

    public ICollection<BEncodedString> Keys => Elements.Keys;
    public ICollection<BEncodedElement> Values => Elements.Values;
}