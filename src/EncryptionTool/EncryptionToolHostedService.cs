using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Volo.Abp;
using Microsoft.Extensions.Configuration;


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

            // TODO: Store and retrieve the encryption key from appsettings.json to avoid asking the user each time
            // Issue URL: https://github.com/suhaib-mousa/encryption-tool/issues/4
        
            var encryptionKey = _configuration["encryptionKey"];
            if (string.IsNullOrEmpty(encryptionKey))
            {
                Console.Write("Enter your encryption key: ");
                encryptionKey = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(encryptionKey))
                {
                    Console.WriteLine("Encryption key cannot be empty. Please try again.");
                    continue;
                }
                _configuration["encryptionKey"] = encryptionKey;
            }
            else
            {
                Console.WriteLine("Using Encryption key from configuration file");
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
                Console.WriteLine($"Error during {operation}: {ex.Message}\n");
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Encryption Tool shutting down...");
        return Task.CompletedTask;
    }
}
