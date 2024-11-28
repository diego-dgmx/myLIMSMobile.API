using LabsoftAPI;

namespace Services {
    public interface IMethodsServices
    {
        Task<ExternalResponse<List<MethodPrerequisiteAnalysisBasic>, ErrorResponse>> MethodPrerequisiteAnalysis(int methodId);
    }
}