using LabsoftAPI;

namespace Interfaces {
    public interface IConsumableTypesServices
    {
        Task<ExternalResponse<List<ConsumableTypeBasic>, ErrorResponse>> GetConsumableTypes(string? identityCenterToken, string? identityCompany);
    }
}