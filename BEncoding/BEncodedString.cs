using System.Text;

namespace Nymphadora.BEncoding;

public class BEncodedString(string value) : BEncodedElement
{
    public string Value { get; set; } = value;

    public BEncodedString() : this("")
    {
    }
    
    public override int Length()
    {
        var length = Encoding.UTF8.GetBytes(Value).Length;
        return length + 1 + length.ToString().Length;
    }

    public override void Decode(IEnumerator<byte> data)
    {
        long length = 0;
        string result = "";
        
        do
        {
            if (!(data.Current >= '0' && data.Current <= '9'))
            {
                throw new FormatException("BEncodedString must start with a number indicating the size of the string.");
            }
            length = length * 10 + (data.Current - '0');
            data.MoveNext();
        } while (data.Current != ':');

        while (length > 0)
        {
            result = result + (char)data.Current;
            length--;
            data.MoveNext();
        }
        Value = result;
    }

    public override byte[] Encode()
    {
        var str = Value.Length.ToString() + ":" + Value;
        return Encoding.UTF8.GetBytes(str);
    }
}