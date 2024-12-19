using LabsoftAPI;

namespace Services {
    public interface IConsumablesServices
    {
        Task<ExternalResponse<MyLIMSResponseBase<ConsumableBasic>, ErrorResponse>> GetConsumables(
            string? identityCenterToken, int? top = null, int? skip = null, string? orderBy = null);
        Task<ExternalResponse<List<SimpleConsumableBasic>, ErrorResponse>> GetConsumablesByConsumableTypeId(int consumableTypeId);
    }
}