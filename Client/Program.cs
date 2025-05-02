namespace Client;

class Program
{
    static async Task Main(string[] args)
    {

        try
        {
            var message = args[0];
            var password = args[1];
            var ipAddress = args[2];
            var port = int.Parse(args[3]);
            var client = new Client(ipAddress,port);

            await client.Connect();
            await client.Send(message,password);
            var data = await client.Receive();
            client.Teardown();

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}