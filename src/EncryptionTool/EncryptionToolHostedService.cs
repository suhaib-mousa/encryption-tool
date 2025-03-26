using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Volo.Abp;
using Microsoft.Extensions.Configuration;
using EncryptionTool.Helpers;


namespace EncryptionTool;

public class EncryptionToolHostedService : IHostedService
{
    private readonly EncryptionService _encryptionService;
    private readonly IConfiguration _configuration;
    public EncryptionToolHostedService(EncryptionService encryptionService, IConfiguration configuration)
    {
        _encryptionService = encryptionService;
        _configuration = configuration;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // TODO enhance the user experience by adding a menu with options to encrypt, decrypt, or exit
        // Issue URL: https://github.com/suhaib-mousa/encryption-tool/issues/5
        Console.WriteLine("Welcome to the Encryption Tool!");
        Console.WriteLine("----------------------------------");

        while (!cancellationToken.IsCancellationRequested)
        {
            Console.Write("Choose operation [encrypt/decrypt/exit]: ");
            var operation = Console.ReadLine()?.Trim().ToLowerInvariant();

            if (string.IsNullOrEmpty(operation) || operation == "exit")
            {
                Console.WriteLine("Exiting the encryption tool...");
                break;
            }

            var encryptionKey = _configuration["encryptionKey"];
            if (string.IsNullOrEmpty(encryptionKey))
            {
                ConsoleHelper.WriteLineInColor("❌ Missing 'encryptionKey' in appsettings.json. Please add it and restart the tool.\n", ConsoleColor.Red);
                continue;
            }

            ConsoleHelper.WriteLineInColor("✅ Using encryption key from configuration.", ConsoleColor.Green);

            Console.Write("Enter the value: ");
            var inputValue = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(inputValue))
            {
                Console.WriteLine("Value cannot be empty. Please try again.");
                continue;
            }

            try
            {
                switch (operation)
                {
                    case "encrypt":
                        _encryptionService.EncryptAndPrint(inputValue, encryptionKey);
                        Console.WriteLine("Encryption successful!\n");
                        break;

                    case "decrypt":
                        _encryptionService.DecryptAndPrint(inputValue, encryptionKey);
                        Console.WriteLine("Decryption successful!\n");
                        break;

                    default:
                        Console.WriteLine("Invalid operation. Please type 'encrypt', 'decrypt', or 'exit'.\n");
                        break;
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.WriteLineInColor($"Error during {operation}: {ex.Message}\n", ConsoleColor.Red);
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Encryption Tool shutting down...");
        return Task.CompletedTask;
    }
}
