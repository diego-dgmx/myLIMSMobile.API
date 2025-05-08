using LabsoftAPI;

namespace Interfaces {
    public interface IMeasurementUnitsServices
    {
        Task<ExternalResponse<List<MeasurementUnitBasic>, ErrorResponse>> GetMeasurementUnits();
    }
}