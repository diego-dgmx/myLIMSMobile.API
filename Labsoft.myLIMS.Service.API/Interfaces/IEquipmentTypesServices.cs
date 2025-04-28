using LabsoftAPI;

namespace Services {
    public interface IEquipmentTypesServices
    {
        Task<ExternalResponse<List<EquipmentType>, ErrorResponse>> GetEquipmentTypes();
    }
}