using LabsoftAPI;
using LabsoftAPI.Auth;

namespace Services {
    public interface ISystemConfigsServices
    {
        Task<ExternalResponse<SampleListInfo, ErrorResponse>> GetSampleListInfo();
    }
}