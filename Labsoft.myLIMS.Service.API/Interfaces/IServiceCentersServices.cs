using Entities;
using LabsoftAPI;

namespace Interfaces
{
    public interface IServiceCentersServices
    {
        Task<ExternalResponse<List<ServiceCenterBasic>, ErrorResponse>> GetServiceCenters();
    }
}