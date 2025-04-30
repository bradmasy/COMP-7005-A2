using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using static Server.Constants;

namespace Server;

public class Server(string ipAddress, int port)
{
    private static readonly byte[] Buffer = new byte[ByteArraySize];
    private readonly Socket _serverSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    private readonly EncryptionService _encryptionService = new EncryptionService();
    public async Task Run()
    {
        try
        {
            BindAndListen();

            Console.WriteLine($"Server is bound to socket: {_serverSocket.LocalEndPoint}");

            while (true)
            {
                var clientSocket = await _serverSocket.AcceptAsync();
                var message = await Read(clientSocket);
              //  var encoded = _encryptionService.Encrypt(message, password);
                Console.WriteLine($"Received: {message}");

                clientSocket.Close();
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

    private void BindAndListen()
    {
        var endpoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
        _serverSocket.Bind(endpoint);
        _serverSocket.Listen(Connections);
    }

    private static async Task<string> Read(Socket client)
    {
        using var ms = new MemoryStream();

        int received;

        while ((received = await client.ReceiveAsync(Buffer, SocketFlags.None)) > 0)
        {
            ms.Write(Buffer, 0, received);
        }

        return Encoding.UTF8.GetString(ms.ToArray());
    }

    // private byte[] Encrypt()
    // {
    //    // var
    // }
    
    
}