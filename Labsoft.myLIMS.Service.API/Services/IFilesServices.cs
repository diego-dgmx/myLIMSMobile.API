using LabsoftAPI;

namespace Services {
    public interface IFilesServices
    {
        Task<ExternalResponse<UploadFileResponse, ErrorResponse>> UploadFile(string? identityCenterToken, IFormFile file);
    }
}