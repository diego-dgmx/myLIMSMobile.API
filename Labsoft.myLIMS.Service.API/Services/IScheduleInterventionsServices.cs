using LabsoftAPI;

namespace Services {
    public interface IScheduleInterventionsServices
    {
        Task<ExternalResponse<MyLIMSResponseBase<ScheduleInterventionBasic>, ErrorResponse>> GetScheduleInterventions(string? sortParam = null, string? filter = null, int? top = null, int? skip = null);
    }
}