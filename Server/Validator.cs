using System.Text.RegularExpressions;
using static BusinessLogic.Constants;

namespace Server;

public static class Validator
{
    private static readonly Regex Ipv4Regex = new Regex(
        @"^(25[0-5]|2[0-4]\d|1\d{2}|[0-9]?\d)" +
        @"(\.(25[0-5]|2[0-4]\d|1\d{2}|[0-9]?\d)){3}$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant
    );
    public static void ValidateArgs(string[] args)
    {
        if (args.Length == NoArgs)
        {
            throw new Exception("Please provide a path to the UNIX domain socket.");
        }

        if (args.Length > AmountOfArgs)
        {
            throw new Exception("Too many arguments provided.");
        }

        if (string.IsNullOrEmpty(args[IpAddress]) || string.IsNullOrWhiteSpace(args[IpAddress]))
        {
            throw new Exception("Null or empty IP Address provided. Please try again.");
        }

        Console.WriteLine($"[{args[IpAddress]}]");
        if (!Ipv4Regex.IsMatch(args[IpAddress]))
        {
            throw new Exception("Invalid IP Address provided. Please try again.");
        }
    }
}