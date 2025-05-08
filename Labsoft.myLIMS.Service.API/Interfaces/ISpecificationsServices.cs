using LabsoftAPI;

namespace Interfaces {
    public interface ISpecificationsServices
    {
        Task<ExternalResponse<List<SpecificationBasic>, ErrorResponse>> GetSpecifications();
    }
}