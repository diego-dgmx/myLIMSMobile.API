using Entities;
using LabsoftAPI;

namespace Services
{
    public interface IServiceAreasServices
    {
        Task<ExternalResponse<List<ServiceAreaBasic>, ErrorResponse>> GetServiceAreas();
    }
}