namespace LabsoftAPI
{
    public class PerformTaskDTO
    {
        public required int SampleMethodId { get; set; }
        public int? SampleMethodCurrentStatusId { get; set; }
        public string? SampleMethodPerformTaskHash { get; set; }
        public DateTime? StartDateMethodAnalysis { get; set; }
        public List<RawDataUpdate>? RawsData { get; set; }
        public List<RawDataChangeHistory>? RawsDataHistory { get; set; }
        public List<AnalysisUpdate>? Analyses { get; set; }
        public List<MethodInfoUpdate>? Infos { get; set; }
        public string? Note { get; set; }
    }

    public class MethodInfoUpdate
    {
        public int? Id { get; set; }
        public int? Order { get; set; }
        public int? MeasurementeUnitId { get; set; }
        public int? ForceScale { get; set; }
        public int? ForceSignifDigits { get; set; }
        public string? ValueText { get; set; }
        public int? ValueInteger { get; set; }
        public float? ValueFloat { get; set; }
        public DateTime? ValueDateTime { get; set; }
        public bool? ValueBoolean { get; set; }
        public int? ValueEquipmentId { get; set; }
        public int? ValueAccountId { get; set; }
        public int? ValueConsumableMovementId { get; set; }
        public int? ValueFileId { get; set; }
        public string? DisplayValue { get; set; }
        public int? MovementConsumableId { get; set; }
        public float? MovementConsumableQuantity { get; set; }
        public string? MovementConsumableNote { get; set; }
        public string? FileIdentification { get; set; }
        public string? FileData { get; set; }
        public string? FileType { get; set; }
    }

    public class AnalysisUpdate
    {
        public int? Id { get; set; }
        public int? Order { get; set; }
        public int? MeasurementeUnitId { get; set; }
        public string? Attribute { get; set; }
        public string? ReferenceMethod { get; set; }
        public float? Uncertainty { get; set; }
        public float? K { get; set; }
        public string? Veff { get; set; }
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
        public int? ValueEquipmentId { get; set; }
        public int? ValueAccountId { get; set; }
        public int? ValueConsumableMovementId { get; set; }
        public int? ValueFileId { get; set; }
        public string? DisplayValue { get; set; }
        public string? DetectionLimitDisplay { get; set; }
        public string? QuantificationLimitDisplay { get; set; }
    }

    public class RawDataChangeHistory
    {
        public int? SampleRawDataId { get; set; }
        public int? MeasurementeUnitId { get; set; }
        public int? ForceScale { get; set; }
        public int? ForceSignifDigits { get; set; }
        public string? DisplayValue { get; set; }
        public string? ValueText { get; set; }
        public int? ValueInteger { get; set; }
        public float? ValueFloat { get; set; }
        public DateTime? ValueDateTime { get; set; }
        public bool? ValueBoolean { get; set; }
        public int? ValueEquipmentId { get; set; }
        public int? ValueAccountId { get; set; }
        public int? ValueConsumableMovementId { get; set; }
        public int? EquipmentId { get; set; }
        public int? ChangeReasonId { get; set; }
        public string? ChangeNote { get; set; }
        public DateTime? EditionDateTime { get; set; }
    }

    public class RawDataUpdate
    {
        public int? Id { get; set; }
        public int? Order { get; set; }
        public int? MeasurementeUnitId { get; set; }
        public int? ForceScale { get; set; }
        public int? ForceSignifDigits { get; set; }
        public string? ValueText { get; set; }
        public int? ValueInteger { get; set; }
        public float? ValueFloat { get; set; }
        public DateTime? ValueDateTime { get; set; }
        public bool? ValueBoolean { get; set; }
        public int? ValueEquipmentId { get; set; }
        public int? ValueAccountId { get; set; }
        public int? ValueConsumableMovementId { get; set; }
        public int? ValueFileId { get; set; }
        public string? DisplayValue { get; set; }
        public string? Tag { get; set; }
        public int? EquipmentId { get; set; }
        public int? MovementConsumableId { get; set; }
        public float? MovementConsumableQuantity { get; set; }
        public string? MovementConsumableNotes { get; set; }
        public string? FileIdentification { get; set; }
        public string? FileData { get; set; }
        public string? FileType { get; set; }
        public int? ChangeReasonId { get; set; }
        public string? ChangeNote { get; set; }
        public int? InfoTypeId { get; set; }
    }
}