using System.Text;

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
        // convert password to number array
        var shiftArray = password.ToCharArray().Select(c => (int)c).ToArray();

        var messageCharArray = message.ToCharArray();

        var index = 0;
        var shiftIndex = 0;

        while (index < message.Length)
        {
            var letter = messageCharArray[index];
            var shift = shiftArray[shiftIndex];

            if (char.IsLetter(letter))
            {
                var charToInt = (int)letter + shift;
                var intToChar = (char)charToInt;
                builder.Append(intToChar);
            }

            index++;
            shiftIndex++;

            if (shiftIndex == shiftArray.Length)
            {
                shiftIndex = 0;
            }
        }

        return builder.ToString();
    }
}