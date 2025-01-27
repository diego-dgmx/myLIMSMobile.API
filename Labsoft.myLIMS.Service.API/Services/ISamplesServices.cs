using LabsoftAPI;

namespace Services {
    public interface ISamplesServices
    {
        Task<ExternalResponse<MyLIMSResponseBase<AnalysisSample>, ErrorResponse>> GetAllSamples(int? sampleType = null, string? sortParam = null, string? filter = null, int? top = null, int? skip = null);
        Task<ExternalResponse<List<AnalysisMethod>, ErrorResponse>> GetAllMethods();
        Task<ExternalResponse<SampleMethodsCount, ErrorResponse>> GetSampleMethodsCount(string? identityCenterToken);
        Task<ExternalResponse<AnalysisSample, ErrorResponse>> ValidateSampleCode(int barCode);
        Task<ExternalResponse<List<QCTest>, ErrorResponse>> GetAvailableQCTestsByRoutineBatchId(int routineBatchId);
        Task<ExternalResponse<List<TaskForPerform>, ErrorResponse>> GetForPerformTask(int[] sampleMethodIds);
        Task<ExternalResponse<string, ErrorResponse>> PerformTask(bool calculate, PerformTaskDTO body);
        Task<ExternalResponse<string, ErrorResponse>> AdvanceStatus(int sampleMethodId, AdvanceMethodStatusParamsDTO body);
    }
}