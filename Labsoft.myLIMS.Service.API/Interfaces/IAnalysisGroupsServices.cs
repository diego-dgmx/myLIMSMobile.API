using LabsoftAPI;

namespace Interfaces {
    public interface IAnalysisGroupsServices
    {
        Task<ExternalResponse<List<AnalysisGroupBasic>, ErrorResponse>> GetAnalysisGroups();
    }
}