namespace Kovan.Application.Common.Interfaces;

public interface IFileStorageService
{
    Task<string> SaveFileAsync(byte[] content, string fileName, string subfolder);
}