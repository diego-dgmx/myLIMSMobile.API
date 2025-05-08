using LabsoftAPI;

namespace Interfaces {
    public interface IInterventionTypesServices
    {
        Task<ExternalResponse<List<InterventionType>, ErrorResponse>> GetInterventionTypes();
    }
}