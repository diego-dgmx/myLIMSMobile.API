using LabsoftAPI;

namespace Services {
    public interface ISamplesServices
    {
        Task<ExternalResponse<MyLIMSResponseBase<AnalysisSample>, ErrorResponse>> GetAllSamples(int? sampleType = null, string? sortParam = null, string? filter = null, int? top = null, int? skip = null);
        Task<ExternalResponse<MyLIMSResponseBase<AnalysisSample>, ErrorResponse>> GetForExecutionSamples(string? identityCenterToken, string? identityCompany, int? sampleType = null, string? sortParam = null, string? filter = null, int? top = null, int? skip = null);
        Task<ExternalResponse<SampleMethodFilterOptions, ErrorResponse>> GetFilterOptions(string? identityCenterToken, string? identityCompany);
        Task<ExternalResponse<List<AnalysisMethod>, ErrorResponse>> GetAllMethods();
        Task<ExternalResponse<SampleMethodsCount, ErrorResponse>> GetSampleMethodsCount(string? identityCenterToken, string? identityCompany, string? methodMasterIds, string? sampleTypeIds, string? methodStatusIds, string? serviceAreaIds, string? sampleReasonIds, string? workIds, string? startUserIds, string? collectPointIds, string? qcTestIds, string? sampleIds, string? sampleIdentification, string? sampleControlNumber, string? validityStartDateTime, string? validityEndDateTime, string? executionStartDateTime, string? executionEndDateTime, string? conclusionStartDateTime, string? conclusionEndDateTime, string? receivedStartDateTime, string? receivedEndDateTime, string? startStartDateTime, string? startEndDateTime, string? takenStartDateTime, string? takenEndDateTime);
        Task<ExternalResponse<AnalysisSample, ErrorResponse>> ValidateSampleCode(int barCode);
        Task<ExternalResponse<List<QCTest>, ErrorResponse>> GetAvailableQCTestsByRoutineBatchId(int routineBatchId);
        Task<ExternalResponse<List<TaskForPerform>, ErrorResponse>> GetForPerformTask(int[] sampleMethodIds);
        Task<ExternalResponse<string, ErrorResponse>> PerformTask(bool calculate, PerformTaskDTO body);
        Task<ExternalResponse<string, ErrorResponse>> AdvanceStatus(int sampleMethodId, AdvanceMethodStatusParamsDTO body);
    }
}