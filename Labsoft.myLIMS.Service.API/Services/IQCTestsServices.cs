using LabsoftAPI;

namespace Services {
    public interface IQCTestsServices
    {
        Task<ExternalResponse<List<QCTest>, ErrorResponse>> GetQCTests(string? identityCenterToken);
        Task<ExternalResponse<List<QCTest>, ErrorResponse>> GetAvailableByQCRoutineBatchId(int id, string? identityCenterToken);
        Task<ExternalResponse<List<int>, ErrorResponse>> AttachSampleMethodsToQCTest(AttachSampleMethodsToQCTestDTO body);
        Task<ExternalResponse<int, ErrorResponse>> CreateNewQCTest(CreateNewQCTestDTO body);
        Task<ExternalResponse<List<QCTestLink>, ErrorResponse>> GetLinkedSamplesByQCTestId(int id, string? identityCenterToken);
        Task<ExternalResponse<List<QCTestLink>, ErrorResponse>> GetControlSamplesByQCTestId(int id, string? identityCenterToken);
    }
}