using System.Text;
using static Server.Constants;

namespace Server;

public class EncryptionService()
{
    public byte[] Encrypt(string data, string password)
    {
        var encrypted = VigenereCipher(data, password);
        return Encoding.UTF8.GetBytes(encrypted);
    }

    private static string VigenereCipher(string message, string password)
    {
        var builder = new StringBuilder();

        var shiftArray = password.ToUpper().ToCharArray().Select(c => c - 'A').ToArray();

        var shiftIndex = 0;

        foreach (var letter in message)
        {
            if (char.IsLetter(letter))
            {
                var offset = char.IsUpper(letter) ? UpperAscii : LowerAscii;
                var shift = shiftArray[shiftIndex];

                var encryptedChar = (char)(((letter - offset + shift) % 26) + offset);
                builder.Append(encryptedChar);

                shiftIndex = (shiftIndex + 1) % shiftArray.Length;
            }
            else // any other char ignore.
            {
                builder.Append(letter);
            }
        }

        return builder.ToString();
    }
}