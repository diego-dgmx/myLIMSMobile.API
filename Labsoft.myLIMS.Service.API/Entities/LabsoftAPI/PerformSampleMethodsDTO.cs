namespace LabsoftAPI
{
    public class AnalysisDataForPerform
    {
        public string? DisplayValue { get; set; }
        public int? ForceScale { get; set; }
        public int? ForceSignifDigits { get; set; }
        public int? Id { get; set; }
        public int? InfoId { get; set; }
        public int? MeasurementUnitId { get; set; }
        public int? MethodId { get; set; }
        public int? SampleId { get; set; }
        public bool? ValueBoolean { get; set; }
        public DateTime? ValueDateTime { get; set; }
        public int? ValueFileId { get; set; }
        public double? ValueFloat { get; set; }
        public int? ValueInteger { get; set; }
        public string? ValueText { get; set; }
        public string? K { get; set; }
        public string? Veff { get; set; }
        public string? Uncertainty { get; set; }
        public string? Info01 { get; set; }
        public string? Info02 { get; set; }
        public string? Info03 { get; set; }
        public string? Info04 { get; set; }
        public string? Info05 { get; set; }
        public string? Info06 { get; set; }
        public string? Info07 { get; set; }
        public string? Info08 { get; set; }
        public string? Info09 { get; set; }
        public string? Info10 { get; set; }
        public int? QuantificationLimit { get; set; }
        public string? QuantificationLimitDisplay { get; set; }
        public int? DetectionLimit { get; set; }
        public string? DetectionLimitDisplay { get; set; }
        public string? ReferenceMethod { get; set; }
        public string? ChangeNote { get; set; }
        public int? ChangeReasonId { get; set; }
    }

    public class MethodForPerform
    {
        public int? Id { get; set; }
        public int? CurrentStatusId { get; set; }
        public int? SampleId { get; set; }
        public int? MethodId { get; set; }
        public string? PerformTaskHash { get; set; }
    }

    public class RawDataForPerform
    {
        public DateTime? EditionDateTime { get; set; }
        public int? EditionUserId { get; set; }
        public int? Id { get; set; }
        public int? InfoId { get; set; }
        public int? InfoTypeId { get; set; }
        public int? MeasurementUnitId { get; set; }
        public int? MethodId { get; set; }
        public int? Order { get; set; }
        public bool? RequiredValue { get; set; }
        public bool? ReadOnlyValue { get; set; }
        public int? SampleId { get; set; }
        public bool? OriginDAQ { get; set; }
        public string? DisplayValue { get; set; }
        public string? ValueText { get; set; }
        public double? ValueFloat { get; set; }
        public int? ValueInteger { get; set; }
        public bool? ValueBoolean { get; set; }
        public DateTime? ValueDateTime { get; set; }
        public int? ValueEquipmentId { get; set; }
        public int? ValueConsumableMovementId { get; set; }
        public int? ValueAccountId { get; set; }
        public int? ValueFileId { get; set; }
        public int? SourceSampleId { get; set; }
        public int? EquipmentId { get; set; }
    }

    public class InfoDataForPerform
    {
        public int? Id { get; set; }
        public int? SId { get; set; }
        public int? SampleMethodId { get; set; }
        public int? EditionUserId { get; set; }
        public DateTime? EditionDateTime { get; set; }
        public string? EditionView { get; set; }
        public int? Order { get; set; }
        public bool? RequiredValue { get; set; }
        public bool? ReadOnlyValue { get; set; }
        public int? InfoId { get; set; }
        public int? InfoTypeId { get; set; }
        public int? MeasurementUnitId { get; set; }
        public string? DisplayValue { get; set; }
        public int? ForceScale { get; set; }
        public int? ForceSignifDigits { get; set; }
        public string? ValueText { get; set; }
        public int? ValueInteger { get; set; }
        public double? ValueFloat { get; set; }
        public DateTime? ValueDateTime { get; set; }
        public bool? ValueBoolean { get; set; }
        public int? ValueEquipmentId { get; set; }
        public int? ValueConsumableMovementId { get; set; }
        public int? ValueFileId { get; set; }
    }

    public class PerformSampleMethodsDTO
    {
        public bool? Debug { get; set; }
        public bool? Advance { get; set; }
        public bool? Calculate { get; set; }
        public bool? ExecuteAnalysisRevisions { get; set; }
        public List<RawDataForPerform>? RawData { get; set; }
        public List<object>? RawDataHistory { get; set; }
        public List<InfoDataForPerform>? Infos { get; set; }
        public List<AnalysisDataForPerform>? AnalysisData { get; set; }
        public List<object>? AnalysisHistory { get; set; }
        public List<TasksNextStatus>? TasksNextStatus { get; set; }
        public List<MethodForPerform>? Methods { get; set; }
        public bool? ShowPopup { get; set; }
    }

    public class TasksNextStatus
    {
        public int? SampleId { get; set; }
        public int? MethodId { get; set; }
        public int? NextStatusId { get; set; }
    }
}