namespace TrackCare.Domain.Interfaces;

public interface IFileStorageService
{
    Task<string> UploadAsync(Stream fileStream, string fileName, string contentType);
    Task<string> GetPublicUrlAsync(string fileName);
    Task DeleteAsync(string fileName);
}
