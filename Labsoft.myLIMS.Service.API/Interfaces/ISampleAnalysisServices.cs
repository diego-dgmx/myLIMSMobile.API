using LabsoftAPI;

namespace Interfaces {
    public interface ISampleAnalysisServices
    {
        Task<ExternalResponse<List<SampleRevisionBasic>, ErrorResponse>> GetRevisions(string? identityCenterToken, string? identityCompany, int[] sampleAnalysisIds);
    }
}