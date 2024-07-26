using LabsoftAPI;

namespace Services {
    public interface ISamplesServices
    {
        Task<ExternalResponse<List<AnalysisSample>, ErrorResponse>> GetAllSamples();
        Task<ExternalResponse<List<AnalysisMethod>, ErrorResponse>> GetAllMethods();
    }
}