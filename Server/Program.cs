using static Server.Constants;

namespace Server;

/**
 * Main program for the server.
 */
class Program
{
    /**
     * The Entry point for the program
     * path should be "/tmp/foo.sock"
     */
    static async Task Main(string[] args)
    {
        try
        {
            Validator.ValidateArgs(args);

            var ipAddress = args[IpAddress];
            var port = int.Parse(args[Port]);
            var server = new Server(ipAddress, port);

            await server.Run();

            server.TearDown();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}