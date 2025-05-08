using LabsoftAPI;

namespace Interfaces {
    public interface IEquipmentTypesServices
    {
        Task<ExternalResponse<List<EquipmentType>, ErrorResponse>> GetEquipmentTypes();
    }
}