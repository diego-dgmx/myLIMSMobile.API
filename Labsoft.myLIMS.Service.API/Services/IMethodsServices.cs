using LabsoftAPI;

namespace Services {
    public interface IMethodsServices
    {
        Task<ExternalResponse<string, ErrorResponse>> AnalysisMethodInstruction(int methodId);
        Task<ExternalResponse<List<MethodPrerequisiteAnalysisBasic>, ErrorResponse>> MethodPrerequisiteAnalysis(int methodId);
    }
}