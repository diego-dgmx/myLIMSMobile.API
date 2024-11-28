namespace LabsoftAPI
{
    public class Analysis
    {
        public int? Id { get; set; }
        public Info? Info { get; set; }
        public MeasurementUnit? MeasurementUnit { get; set; }
        public int? Order { get; set; }
        public string? DisplayValue { get; set; }
        public int? ForceScale { get; set; }
        public object? ForceSignifDigits { get; set; }
        public object? ValueText { get; set; }
        public object? ValueInteger { get; set; }
        public double? ValueFloat { get; set; }
        public object? ValueDateTime { get; set; }
        public object? ValueBoolean { get; set; }
        public object? ValueFileId { get; set; }
        public object? Uncertainty { get; set; }
        public object? K { get; set; }
        public object? Veff { get; set; }
        public object? DetectionLimit { get; set; }
        public object? QuantificationLimit { get; set; }
        public object? DetectionLimitDisplay { get; set; }
        public object? QuantificationLimitDisplay { get; set; }
        public object? Info01 { get; set; }
        public object? Info02 { get; set; }
        public object? Info03 { get; set; }
        public object? Info04 { get; set; }
        public object? Info05 { get; set; }
        public object? Info06 { get; set; }
        public object? Info07 { get; set; }
        public object? Info08 { get; set; }
        public object? Info09 { get; set; }
        public object? Info10 { get; set; }
        public string? ReferenceMethod { get; set; }
        public MeasurementUnit? SampleAnalysisConclusion { get; set; }
        public object? UpperLimit { get; set; }
        public object? LowerLimit { get; set; }
        public bool? ObrigateNotConformityConclusionMessage { get; set; }
    }

    public class Info
    {
        public int? Id { get; set; }
        public string? Identification { get; set; }
        public int? InfoTypeId { get; set; }
        public bool? Active { get; set; }
        public object? ForceScale { get; set; }
        public object? ForceSignifDigits { get; set; }
        public bool? ReadOnlyValue { get; set; }
        public object? AccountTypeId { get; set; }
        public bool? AllowAnyValue { get; set; }
        public bool? AllowText { get; set; }
        public EquipmentType? EquipmentType { get; set; }
        public ConsumableType? ConsumableType { get; set; }
        public List<object>? Options { get; set; }
    }

    public class Method
    {
        public int? Id { get; set; }
        public string? Identification { get; set; }
        public int? MasterId { get; set; }
        public object? QCRoutineBatchIds { get; set; }
    }

    public class NextStatus
    {
        public int? Id { get; set; }
        public string? Identification { get; set; }
        public bool? Default { get; set; }
    }

    public class RawsData
    {
        public int? Id { get; set; }
        public Info? Info { get; set; }
        public MeasurementUnit? MeasurementUnit { get; set; }
        public int? Order { get; set; }
        public Method? Method { get; set; }
        public string? DisplayValue { get; set; }
        public int? ForceScale { get; set; }
        public object? ForceSignifDigits { get; set; }
        public object? ValueText { get; set; }
        public object? ValueInteger { get; set; }
        public double? ValueFloat { get; set; }
        public object? ValueDateTime { get; set; }
        public object? ValueBoolean { get; set; }
        public object? ValueEquipmentId { get; set; }
        public object? ValueConsumableMovementId { get; set; }
        public object? ValueConsumableMovement { get; set; }
        public object? ValueFileId { get; set; }
        public object? ValueAccountId { get; set; }
        public object? Tag { get; set; }
        public object? MethodStatusId { get; set; }
        public bool? MethodStatusVisible { get; set; }
        public object? DAQId { get; set; }
        public object? DAQIdentification { get; set; }
        public object? DAQIdentificationTag { get; set; }
        public object? DAQDateTime { get; set; }
        public bool? OriginDAQ { get; set; }
        public int? InfoTypeId { get; set; }
        public object? EquipmentId { get; set; }
        public object? Equipment { get; set; }
        public object? EquipmentTypeId { get; set; }
        public EquipmentType? EquipmentType { get; set; }
        public object? RawDataOriginId { get; set; }
        public object? InfoRawDataOriginId { get; set; }
        public object? SampleTypeRawDataOriginId { get; set; }
        public object? RawDataInterventionOriginId { get; set; }
        public object? InterventionTypeId { get; set; }
        public object? RawDataConsumableOriginId { get; set; }
        public object? FileIdentification { get; set; }
        public object? FileData { get; set; }
        public bool? RequiredValue { get; set; }
        public bool? ReadOnlyValue { get; set; }
        public DateTime? EditionDateTime { get; set; }
        public EditionUser? EditionUser { get; set; }
        public int? SampleId { get; set; }
    }

    public class ConsumableType
    {
        public int Id { get; set; }
        public string? Identification { get; set; }
        public MeasurementUnit? MeasurementUnit { get; set; }
    }

    public class MeasurementUnit
    {
        public int? Id { get; set; }
        public string? Identification { get; set; }
    }

    public class RelatedAccount
    {
        public int? Id { get; set; }
        public string? Identification { get; set; }
    }

    public class TaskForPerform
    {
        public SampleMethod? SampleMethod { get; set; }
        public List<RawsData>? RawsData { get; set; }
        public List<Analysis>? Analyses { get; set; }
        public List<object>? Infos { get; set; }
        public List<object>? SampleInfos { get; set; }
        public List<object>? LogDAQs { get; set; }
    }

    public class SampleMethod
    {
        public int Id { get; set; }
        public Sample? Sample { get; set; }
        public Method? Method { get; set; }
        public ServiceArea? ServiceArea { get; set; }
        public CurrentStatus? CurrentStatus { get; set; }
        public object? AnalysisDeadline { get; set; }
        public object? Conclusion { get; set; }
        public string? PerformTaskHash { get; set; }
        public object? QCTests { get; set; }
        public bool? Offline { get; set; }
        public List<NextStatus>? NextStatus { get; set; }
        public bool? MobileSampling { get; set; }
    }
}