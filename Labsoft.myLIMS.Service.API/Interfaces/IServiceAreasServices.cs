using Entities;
using LabsoftAPI;

namespace Interfaces
{
    public interface IServiceAreasServices
    {
        Task<ExternalResponse<List<ServiceAreaBasic>, ErrorResponse>> GetServiceAreas();
    }
}