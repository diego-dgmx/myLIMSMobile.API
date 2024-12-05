using LabsoftAPI;

namespace Services {
    public interface ISamplesServices
    {
        Task<ExternalResponse<List<AnalysisSample>, ErrorResponse>> GetAllSamples(int? sampleType = null);
        Task<ExternalResponse<List<AnalysisMethod>, ErrorResponse>> GetAllMethods();
        Task<ExternalResponse<AnalysisSample, ErrorResponse>> ValidateSampleCode(int barCode);
        Task<ExternalResponse<List<QCTest>, ErrorResponse>> GetAvailableQCTestsByRoutineBatchId(int routineBatchId);
        Task<ExternalResponse<List<TaskForPerform>, ErrorResponse>> GetForPerformTask(int[] sampleMethodIds);
        Task<ExternalResponse<string, ErrorResponse>> PerformTask(bool calculate, PerformTaskDTO body);
        Task<ExternalResponse<string, ErrorResponse>> AdvanceStep(PerformTaskBackgroundParamsDTO body);
    }
}