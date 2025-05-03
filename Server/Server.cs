using System.Net;
using System.Net.Sockets;
using System.Text;
using BusinessLogic;
using static BusinessLogic.Constants;

namespace Server;

public class Server(string ipAddress, int port)
{
    private static readonly byte[] Buffer = new byte[ByteArraySize];
    private readonly Socket _serverSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    public async Task Run()
    {
        try
        {
            BindAndListen();

            Console.WriteLine($"Server is bound to socket: {_serverSocket.LocalEndPoint}");

            while (true)
            {
                var clientSocket = await _serverSocket.AcceptAsync();
                await HandleClient(clientSocket);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public void TearDown()
    {
        _serverSocket.Close();
    }

    private static async Task HandleClient(Socket clientSocket)
    {
        try
        {
            var message = await Read(clientSocket);
            var processed = ProcessMessage(message);
            var encoded = EncryptionService.Encrypt(processed[Message], processed[Password]);

            await Send(clientSocket, encoded);

            Console.WriteLine(
                $"Message processed | Original: [{processed[Message]}] Encoded:[{Encoding.ASCII.GetString(encoded)}]");
        }
        catch (Exception ex)
        {
            var errorResponse = Encoding.ASCII.GetBytes($"Server error: {ex.Message}");
            await Send(clientSocket, errorResponse);
        }
        finally
        {
            Console.WriteLine("Closing client connection");
            clientSocket.Close();
        }
    }

    private static async Task Send(Socket client, byte[] data)
    {
        var descriptor = await client.SendAsync(data, SocketFlags.None);
        if (descriptor <= 0)
        {
            throw new Exception("Error transmitting message to client.");
        }
    }

    private static string[] ProcessMessage(string message)
    {
        var messageParts = message.Split(Delimiter);
        if (messageParts.Length < ExpectedMessages) throw new Exception($"Invalid message: {message}");
        return messageParts;
    }


    private void BindAndListen()
    {
        var endpoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
        _serverSocket.Bind(endpoint);
        _serverSocket.Listen(Connections);
    }

    private static async Task<string> Read(Socket client)
    {
        var received = await client.ReceiveAsync(Buffer, SocketFlags.None);
        return received > 0 ? Encoding.UTF8.GetString(Buffer, 0, received) : string.Empty;
    }
}