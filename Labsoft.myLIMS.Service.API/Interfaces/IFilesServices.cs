using LabsoftAPI;

namespace Interfaces {
    public interface IFilesServices
    {
        Task<ExternalResponse<UploadFileResponse, ErrorResponse>> UploadFile(string? identityCenterToken, string? identityCompany, IFormFile file);
        Task<ExternalResponse<string, ErrorResponse>> GetFileData(int fileId);
    }
}