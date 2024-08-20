using LabsoftAPI;

namespace Services {
    public interface ISamplesServices
    {
        Task<ExternalResponse<List<AnalysisSample>, ErrorResponse>> GetAllSamples(int? sampleType = null);
        Task<ExternalResponse<List<AnalysisMethod>, ErrorResponse>> GetAllMethods();
    }
}