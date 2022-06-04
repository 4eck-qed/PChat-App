using Google.Protobuf;

namespace PChat.Extensions;

public static class ByteStringExtensions
{
    public static string ToHexString(this ByteString? byteString)
    {
        return byteString == null ? string.Empty : HexString.Parse(byteString.ToByteArray());
    }
    
    public static ByteString RandomByteString(int size)
    {
        var bytes = new byte[size];
        new Random().NextBytes(bytes);
        return ByteString.CopyFrom(bytes);
    }
}