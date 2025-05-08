using LabsoftAPI;
using LabsoftAPI.Auth;

namespace Interfaces {
    public interface ISystemConfigsServices
    {
        Task<ExternalResponse<SampleListInfo, ErrorResponse>> GetSampleListInfo();
    }
}