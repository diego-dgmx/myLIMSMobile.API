using LabsoftAPI;

namespace Services {
    public interface IScheduleInterventionsServices
    {
        Task<ExternalResponse<MyLIMSResponseBase<ScheduleInterventionBasic>, ErrorResponse>> GetScheduleInterventions(string? sortParam = null, string? filter = null, int? top = null, int? skip = null);
        Task<ExternalResponse<List<ScheduleAnalysisGroupBasic>, ErrorResponse>> GetScheduleAnalysisGroups(string? identityCenterToken, string? identityCompany, int id);
        Task<ExternalResponse<List<ScheduleSpecificationBasic>, ErrorResponse>> GetScheduleSpecifications(string? identityCenterToken, string? identityCompany, int id);
        Task<ExternalResponse<List<ScheduleSampleInterventionBasic>, ErrorResponse>> GetScheduleSampleInterventions(string? identityCenterToken, string? identityCompany, int id);
        Task<ExternalResponse<ScheduleControlPlanBasic, ErrorResponse>> GetScheduleControlPlan(string? identityCenterToken, string? identityCompany, int id);

    }
}