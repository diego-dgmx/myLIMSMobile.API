using LabsoftAPI;

namespace Services {
    public interface IConsumablesServices
    {
        Task<ExternalResponse<MyLIMSResponseBase<ConsumableBasic>, ErrorResponse>> GetConsumables(
            string? identityCenterToken, string? identityCompany, int? top = null, int? skip = null, string? filter = null, string? orderBy = null);
        Task<ExternalResponse<ConsumableBasic, ErrorResponse>> GetConsumable(
            string? identityCenterToken, string? identityCompany, int id);
        Task<ExternalResponse<List<ConsumableInfoBasic>, ErrorResponse>> GetConsumableInfos(int id);
        Task<ExternalResponse<List<ConsumableMovementBasic>, ErrorResponse>> GetConsumableMovements(
            string? identityCenterToken, string? identityCompany, int id);
        Task<ExternalResponse<List<ConsumableServiceAreaBasic>, ErrorResponse>> GetConsumableServiceAreas(
            string? identityCenterToken, string? identityCompany, int id);
        Task<ExternalResponse<List<ConsumableServiceCenterBasic>, ErrorResponse>> GetConsumableServiceCenters(
            string? identityCenterToken, string? identityCompany, int id);
        Task<ExternalResponse<List<SimpleConsumableBasic>, ErrorResponse>> GetConsumablesByConsumableTypeId(int consumableTypeId);
        Task<ExternalResponse<dynamic, ErrorResponse>> SetConsumptionInAnalysis(ConsumableConsumptionInAnalysisDTO body);
        Task<ExternalResponse<string, ErrorResponse>> InactivateMovement(
            string? identityCenterToken, string? identityCompany, int consumableId, int movementId, UpdateMovementDTO body);
        Task<ExternalResponse<string, ErrorResponse>> ActivateMovement(
            string? identityCenterToken, string? identityCompany, int consumableId, int movementId, UpdateMovementDTO body);
        Task<ExternalResponse<dynamic, ErrorResponse>> CreateConsumableSample(CreateConsumableSampleDTO body);
    }
}