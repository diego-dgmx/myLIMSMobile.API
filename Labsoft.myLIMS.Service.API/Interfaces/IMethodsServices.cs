using LabsoftAPI;

namespace Interfaces {
    public interface IMethodsServices
    {
        Task<ExternalResponse<List<AnalysisMethod>, ErrorResponse>> GetMethods();
        Task<ExternalResponse<List<MethodAnalysisBasic>, ErrorResponse>> GetMethodAnalysisByMethodId(int methodId);
        Task<ExternalResponse<string, ErrorResponse>> AnalysisMethodInstruction(int methodId);
        Task<ExternalResponse<List<MethodPrerequisiteAnalysisBasic>, ErrorResponse>> MethodPrerequisiteAnalysis(int methodId);
    }
}