using UploadApi.DTO;

namespace UploadApi.Service.AzureBlobStorage
{
    public interface IBookUploadService
    {
        Task<UploadResponse> UploadPDFAsync(UploadContentRequest formData, string bookFolder, string imgFolder);
    }
}
