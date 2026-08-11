using Kovan.Application.Common.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace Kovan.Infrastructure.Services;

public class FileStorageService : IFileStorageService
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public FileStorageService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<string> SaveFileAsync(byte[] content, string fileName, string subfolder)
    {
        var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
        var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, subfolder);

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        var filePath = Path.Combine(folderPath, uniqueFileName);
        await File.WriteAllBytesAsync(filePath, content);

        // Sadece göreli yolu döndür
        return Path.Combine(subfolder, uniqueFileName).Replace(Path.DirectorySeparatorChar, '/');
    }
}