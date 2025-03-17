using LabsoftAPI;

namespace Services {
    public interface IConsumablesServices
    {
        Task<ExternalResponse<MyLIMSResponseBase<ConsumableBasic>, ErrorResponse>> GetConsumables(
            string? identityCenterToken, int? top = null, int? skip = null, string? filter = null, string? orderBy = null);
        Task<ExternalResponse<ConsumableBasic, ErrorResponse>> GetConsumable(
            string? identityCenterToken, int id);
        Task<ExternalResponse<List<ConsumableInfoBasic>, ErrorResponse>> GetConsumableInfos(int id);
        Task<ExternalResponse<List<ConsumableMovementBasic>, ErrorResponse>> GetConsumableMovements(
            string? identityCenterToken, int id);
        Task<ExternalResponse<List<ConsumableServiceAreaBasic>, ErrorResponse>> GetConsumableServiceAreas(
            string? identityCenterToken, int id);
        Task<ExternalResponse<List<ConsumableServiceCenterBasic>, ErrorResponse>> GetConsumableServiceCenters(
            string? identityCenterToken, int id);
        Task<ExternalResponse<List<SimpleConsumableBasic>, ErrorResponse>> GetConsumablesByConsumableTypeId(int consumableTypeId);
        Task<ExternalResponse<dynamic, ErrorResponse>> SetConsumptionInAnalysis(ConsumableConsumptionInAnalysisDTO body);
        Task<ExternalResponse<string, ErrorResponse>> InactivateMovement(
            string? identityCenterToken, int consumableId, int movementId, UpdateMovementDTO body);
        Task<ExternalResponse<string, ErrorResponse>> ActivateMovement(
            string? identityCenterToken, int consumableId, int movementId, UpdateMovementDTO body);
        Task<ExternalResponse<dynamic, ErrorResponse>> CreateConsumableSample(CreateConsumableSampleDTO body);
    }
}