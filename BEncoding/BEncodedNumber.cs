namespace Nymphadora.BEncoding;

public class BEncodedNumber(long value) : BEncodedElement
{
    private long Value { get; set; } = value;

    public BEncodedNumber() : this(0)
    {
    }

    public override int Length() => Value.ToString().Length + 2;

    public override void Decode(IEnumerator<byte> data)
    {
        long result = 0;
        var sign = 1;
        if (data.Current != 'i')
        {
            throw new FormatException("BEncodedNumber must start with 'i'.");
        }

        data.MoveNext();
        if (data.Current == 'e')
        {
            throw new FormatException("BEncodedNumber must have at least one digit.");
        }

        if (data.Current == '-')
        {
            sign = -1;
            data.MoveNext();
        }

        do
        {
            if (data.Current == 'e')
            {
                break;
            }

            if (data.Current < '0' || data.Current > '9')
            {
                throw new FormatException("BEncodedNumber must contain only numeric digits or '-'.");
            }
            
            result = result * 10 + (data.Current - '0');
        } while(data.MoveNext());

        data.MoveNext();
        Value = result * sign;
    }

    public override byte[] Encode()
    {
        var str = "i" + Value + "e";
        return System.Text.Encoding.UTF8.GetBytes(str);
    }
}