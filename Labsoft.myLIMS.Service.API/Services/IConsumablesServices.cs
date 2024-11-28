using LabsoftAPI;

namespace Services {
    public interface IConsumablesServices
    {
        Task<ExternalResponse<List<Consumable>, ErrorResponse>> GetConsumablesByConsumableTypeId(int consumableTypeId);
    }
}