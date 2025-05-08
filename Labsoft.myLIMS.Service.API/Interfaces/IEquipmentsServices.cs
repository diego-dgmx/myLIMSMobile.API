using LabsoftAPI;

namespace Interfaces {
    public interface IEquipmentsServices
    {
        Task<ExternalResponse<List<Equipment>, ErrorResponse>> GetEquipmentsByEquipmentTypeId(int equipmentTypeId);
        Task<ExternalResponse<List<Equipment>, ErrorResponse>> GetEquipmentsByServiceCenters();
    }
}