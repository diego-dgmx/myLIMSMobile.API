using LabsoftAPI;

namespace Services {
    public interface ISampleAnalysisServices
    {
        Task<ExternalResponse<List<SampleRevisionBasic>, ErrorResponse>> GetRevisions(string? identityCenterToken, string? identityCompany, int[] sampleAnalysisIds);
    }
}