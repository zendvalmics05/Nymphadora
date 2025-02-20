using System.Collections;

namespace Nymphadora.BEncoding;

public class BEncodedList(List<BEncodedElement> elements) : BEncodedElement, IList<BEncodedElement>
{
    private List<BEncodedElement> Elements { get; set; } = elements;

    public BEncodedList() : this([])
    {
    }

    public override int Length() => 2 + Elements.Sum(element => element.Length());

    public override void Decode(IEnumerator<byte> data)
    {
        var list = new BEncodedList();
        while (data.MoveNext())
        {
            if (data.Current == 'e')
            {
                Elements = list.Elements;
                return;
            }
            list.Add(BEncodeDecoder.Decode(data));
        }
    }

    public override byte[] Encode()
    {
        var length = Elements.Sum(element => element.Length());

        length += 2;
        
        var arr = new byte[length];

        arr[0] = (byte)'l';

        var idx = 1;

        foreach (var b in Elements.Select(element => element.Encode()).SelectMany(encodedElement => encodedElement))
        {
            arr[idx++] = b;
        }
        arr[idx] = (byte)'e';
        return arr;
    }

    public IEnumerator<BEncodedElement> GetEnumerator() => Elements.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Add(BEncodedElement item) => Elements.Add(item);

    public void Clear() => Elements.Clear();

    public bool Contains(BEncodedElement item) => Elements.Contains(item);

    public void CopyTo(BEncodedElement[] array, int arrayIndex) => Elements.CopyTo(array, arrayIndex);

    public bool Remove(BEncodedElement item) => Elements.Remove(item);

    public int Count => Elements.Count;
    public bool IsReadOnly => false;

    public int IndexOf(BEncodedElement item) => Elements.IndexOf(item);

    public void Insert(int index, BEncodedElement item) => Elements.Insert(index, item);

    public void RemoveAt(int index) => Elements.RemoveAt(index);

    public BEncodedElement this[int index]
    {
        get => Elements[index];
        set => Elements[index] = value;
    }
}