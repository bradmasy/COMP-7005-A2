# COMP 7005 Assignment 2

A simple client-server application that encrypts messages using the Vigenère cipher over network sockets.

## Setup

1. Make sure you have .NET 8.0 installed
2. Open two terminal windows
3. Build the project:
```bash
dotnet build
```

## How to Run

### Server
```bash
cd Server
dotnet run <ip-address> <port>
```
Example: `dotnet run 192.168.0.1 9876`

### Client
```bash
cd Client
dotnet run <message> <key> <server-ip> <port>
```
Example: `dotnet run "Hello World" "PASSWORD" 192.168.0.1 9876`

## How it Works

1. Server starts and listens for connections
2. Client connects and sends a message with format: `message|key`
3. Server encrypts the message and sends it back
4. Client decrypts the message locally

## Features

- IPv4 socket communication
- Vigenère cipher encryption
- Handles one client at a time
- Preserves case and non-alphabetic characters
- Basic error handling

## Testing

See TestPlan.md for test cases and procedures.

## Troubleshooting

If you get connection errors:
- Check if server is running
- Verify IP and port numbers
- Try: `sudo route delete <ip-address>` on macOS

## Notes

- Built for COMP 7005 Assignment 2
- Uses only standard .NET libraries
- No external encryption libraries
- For educational purposes only
