using LabsoftAPI;

namespace Services {
    public interface IEquipmentsServices
    {
        Task<ExternalResponse<List<Equipment>, ErrorResponse>> GetEquipmentsByEquipmentTypeId(int equipmentTypeId);
    }
}