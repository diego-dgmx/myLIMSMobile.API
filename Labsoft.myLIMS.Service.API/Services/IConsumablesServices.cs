using LabsoftAPI;

namespace Services {
    public interface IConsumablesServices
    {
        Task<ExternalResponse<MyLIMSResponseBase<ConsumableBasic>, ErrorResponse>> GetConsumables(
            string? identityCenterToken, int? top = null, int? skip = null, string? orderBy = null);
        Task<ExternalResponse<ConsumableBasic, ErrorResponse>> GetConsumable(
            string? identityCenterToken, int id);
        Task<ExternalResponse<List<ConsumableMovementBasic>, ErrorResponse>> GetConsumableMovements(
            string? identityCenterToken, int id);
        Task<ExternalResponse<List<ConsumableServiceAreaBasic>, ErrorResponse>> GetConsumableServiceAreas(
            string? identityCenterToken, int id);
        Task<ExternalResponse<List<SimpleConsumableBasic>, ErrorResponse>> GetConsumablesByConsumableTypeId(int consumableTypeId);
        Task<ExternalResponse<dynamic, ErrorResponse>> SetConsumptionInAnalysis(ConsumableConsumptionInAnalysisDTO body);
    }
}