using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Encryption;

namespace EncryptionTool;

public class EncryptionService : ITransientDependency
{
    private readonly IStringEncryptionService _stringEncryptionService;

    public EncryptionService(IStringEncryptionService stringEncryptionService)
    {
        _stringEncryptionService = stringEncryptionService;
    }

    public void EncryptAndPrint(string rawValue, string encryptionKey)
    {
        string encryptedValue = _stringEncryptionService.Encrypt(rawValue, encryptionKey)!;
        Console.WriteLine($"Encrypted value: {encryptedValue}");
    }
}