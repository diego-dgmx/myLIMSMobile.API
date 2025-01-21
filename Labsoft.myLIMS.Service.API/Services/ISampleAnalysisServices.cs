using LabsoftAPI;

namespace Services {
    public interface ISampleAnalysisServices
    {
        Task<ExternalResponse<List<SampleRevisionBasic>, ErrorResponse>> GetRevisions(string? identityCenterToken, int[] sampleAnalysisIds);
    }
}