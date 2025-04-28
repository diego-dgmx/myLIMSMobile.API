using LabsoftAPI;

namespace Services {
    public interface IMeasurementUnitsServices
    {
        Task<ExternalResponse<List<MeasurementUnitBasic>, ErrorResponse>> GetMeasurementUnits();
    }
}