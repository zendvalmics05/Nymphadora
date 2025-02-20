namespace Nymphadora.BEncoding;

public abstract class BEncodedElement
{
    public abstract int Length();
    public abstract void Decode(IEnumerator<byte> data);
    public abstract byte[] Encode();
}