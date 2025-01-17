using LabsoftAPI;

namespace Services {
    public interface IConsumableTypesServices
    {
        Task<ExternalResponse<List<ConsumableTypeBasic>, ErrorResponse>> GetConsumableTypes(string? identityCenterToken);
    }
}