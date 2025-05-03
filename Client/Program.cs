using BusinessLogic;
using Server;
using static BusinessLogic.Constants;

namespace Client;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            Validator.ValidateClientArguments(args);

            var message = args[Message];
            var password = args[Password];
            var ipAddress = args[IpAddressIndex];
            var port = int.Parse(args[PortIndex]);

            var client = new Client(ipAddress, port);

            await client.Connect();
            await client.Send(message, password);

            var data = await client.Receive();
            var decrypted = EncryptionService.Decrypt(data, password);

            client.DisplayMessage(decrypted);
            client.Teardown();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}