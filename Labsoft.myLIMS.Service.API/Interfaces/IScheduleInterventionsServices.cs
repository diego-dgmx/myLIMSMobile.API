using LabsoftAPI;

namespace Interfaces {
    public interface IScheduleInterventionsServices
    {
        Task<ExternalResponse<MyLIMSResponseBase<ScheduleInterventionBasic>, ErrorResponse>> GetScheduleInterventions(string? sortParam = null, string? filter = null, int? top = null, int? skip = null);
        Task<ExternalResponse<ScheduleInterventionBasic, ErrorResponse>> GetScheduleIntervention(string? identityCenterToken, string? identityCompany, int id);
        Task<ExternalResponse<List<ScheduleAnalysisGroupBasic>, ErrorResponse>> GetScheduleAnalysisGroups(string? identityCenterToken, string? identityCompany, int id);
        Task<ExternalResponse<List<ScheduleSpecificationBasic>, ErrorResponse>> GetScheduleSpecifications(string? identityCenterToken, string? identityCompany, int id);
        Task<ExternalResponse<List<ScheduleSampleInterventionBasic>, ErrorResponse>> GetScheduleSampleInterventions(string? identityCenterToken, string? identityCompany, int id);
        Task<ExternalResponse<ScheduleControlPlanBasic, ErrorResponse>> GetScheduleControlPlan(string? identityCenterToken, string? identityCompany, int id);
        Task<ExternalResponse<int, ErrorResponse>> CreateScheduleIntervention(string? identityCenterToken, string? identityCompany, ScheduleInterventionCreateDTO body);
        Task<ExternalResponse<dynamic, ErrorResponse>> UpdateScheduleIntervention(string? identityCenterToken, string? identityCompany, ScheduleInterventionUpdateDTO body);
        Task<ExternalResponse<int, ErrorResponse>> CreateScheduleInterventionRoutine(string? identityCenterToken, string? identityCompany, ScheduleInterventionDTO body);
        Task<ExternalResponse<int, ErrorResponse>> CreateScheduleInterventionExtraordinary(string? identityCenterToken, string? identityCompany, ScheduleInterventionDTO body);
    }
}