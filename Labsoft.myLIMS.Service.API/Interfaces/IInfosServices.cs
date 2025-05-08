using LabsoftAPI;

namespace Interfaces {
    public interface IInfosServices
    {
        Task<ExternalResponse<MyLIMSResponseBase<InfoBasic>, ErrorResponse>> GetInfos(int? top = null, int? skip = null, string? filter = null);
    }
}