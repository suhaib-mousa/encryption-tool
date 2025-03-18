using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Volo.Abp;

namespace EncryptionTool;

public class EncryptionToolHostedService : IHostedService
{
    private readonly EncryptionService _encryptionService;
    public EncryptionToolHostedService(EncryptionService encryptionService)
    {
        _encryptionService = encryptionService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Welcome to the Encryption Tool!");
        Console.WriteLine("----------------------------------");

        while (!cancellationToken.IsCancellationRequested)
        {
            Console.Write("Enter the value to encrypt (or type 'exit' to quit): ");
            var rawValue = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(rawValue) || rawValue.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Exiting the encryption tool...");
                break;
            }

            Console.Write("Enter your encryption key: ");
            var encryptionKey = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(encryptionKey))
            {
                Console.WriteLine("Encryption key cannot be empty. Please try again.");
                continue;
            }

            try
            {
                _encryptionService.EncryptAndPrint(rawValue, encryptionKey);
                Console.WriteLine("Encryption successful!\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during encryption: {ex.Message}\n");
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Encryption Tool shutting down...");
        return Task.CompletedTask;
    }
}
