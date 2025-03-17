using Entities;
using LabsoftAPI;

namespace Services
{
    public interface IServiceCentersServices
    {
        Task<ExternalResponse<List<ServiceCenterBasic>, ErrorResponse>> GetServiceCenters();
    }
}