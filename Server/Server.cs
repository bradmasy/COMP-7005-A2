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

                try
                {
                    Console.WriteLine("incoming client");
                    var message = await Read(clientSocket);
                    Console.WriteLine("message");
                    var processed = ProcessMessage(message);
                    var encoded = EncryptionService.Encrypt(processed[Message], processed[Password]);
                    var descriptor = await Send(clientSocket, encoded);
                    Console.WriteLine($"Received: {message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    var encodedError = Encoding.ASCII.GetBytes(ex.Message);
                    await Send(clientSocket, encodedError);
                }
                finally
                {
                    Console.WriteLine("Closing");
                    clientSocket.Close();
                }
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

    private async Task<int> Send(Socket client, byte[] data)
    {
        var descriptor = await client.SendAsync(data, SocketFlags.None);
        return descriptor;
    }

    private static string[] ProcessMessage(string message)
    {
        var messageParts = message.Split("|");
        if (messageParts.Length < 2) throw new Exception($"Invalid message: {message}");
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
        using var ms = new MemoryStream();
        var received = await client.ReceiveAsync(Buffer, SocketFlags.None);

        if (received > 0)
        {
            ms.Write(Buffer, 0, received);
        }

        var result = Encoding.UTF8.GetString(ms.ToArray());
        Console.WriteLine($"Message content: {result}");
        return result;
    }

    // private byte[] Encrypt()
    // {
    //    // vaΩΩr
    // }
}