using System.Text;
using System.Text.RegularExpressions;
using Google.Protobuf;

// ReSharper disable once CheckNamespace
namespace PChat;

public static class HexString
{
    private static readonly char[] HexChars =
        {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};

    private static readonly Dictionary<char, int> HexTable = new()
    {
        {'0', 0}, {'1', 1}, {'2', 2}, {'3', 3}, {'4', 4}, {'5', 5}, {'6', 6}, {'7', 7},
        {'8', 8}, {'9', 9}, {'A', 10}, {'B', 11}, {'C', 12}, {'D', 13}, {'E', 14}, {'F', 15}
    };


    private static bool IsValid(string hexString)
    {
        var regex = new Regex("[0-9A-F]+");
        return regex.IsMatch(hexString) && hexString.Length % 2 == 0;
    }

    public static string Parse(uint @decimal)
    {
        var result = new StringBuilder();
        var quotient = @decimal;
        do
        {
            var remainder = quotient % 16;
            quotient /= 16;
            if (remainder > 15)
            {
                result.Append(Parse(remainder));
            }
            else
            {
                result.Append(HexChars[remainder]);
            }
        } while (quotient != 0);

        // reverse order
        for (int i = 0, n = result.Length; i < n / 2; i++)
        {
            (result[i], result[n - i - 1]) = (result[n - i - 1], result[i]);
        }
        return result.ToString();
    }

    public static string Parse(IEnumerable<byte> bytes)
    {
        var hexString = new StringBuilder();
        foreach (var @byte in bytes)
        {
            var currentHex = Parse(@byte);

            //if single character
            if (currentHex.Length == 1)
            {
                currentHex = currentHex.Insert(0, "0");
            }

            hexString.Append(currentHex);
        }

        return hexString.ToString();
    }

    public static int? ToDecimal(string hexString)
    {
        if (!IsValid(hexString))
        {
            Console.WriteLine($"[HexString.ToDecimal]: hexString '{hexString}' is not a valid hexadecimal!");
            return null;
        }

        var result = 0;
        for (int i = 0, n = hexString.Length - 1; i < hexString.Length; i++, n--)
        {
            result += (int) (HexTable[hexString[i]] * Math.Pow(16, n));
        }

        return result;
    }

    public static ByteString? ToByteString(string hexString)
    {
        if (!IsValid(hexString))
        {
            Console.WriteLine($"[HexString.ToByteString]: hexString '{hexString}' is not a valid hexadecimal!");
            return null;
        }

        var result = new List<byte>();

        for (var i = 0; i < hexString.Length - 1; i += 2)
        {
            var currentHex = hexString.Substring(i, 2);
            var asDecimal = ToDecimal(currentHex);
            if (asDecimal == null || asDecimal.ToString() == null)
            {
                Console.WriteLine("[HexString.ToByteString]: asDecimal is null!");
                return null;
            }

            result.Add(byte.Parse(asDecimal.ToString()!));
        }

        return ByteString.CopyFrom(result.ToArray());
    }
}