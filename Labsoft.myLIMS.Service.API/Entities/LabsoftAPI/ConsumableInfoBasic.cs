namespace LabsoftAPI {
    public class ConsumableInfoBasic
    {
        public int? Id { get; set; }
        public bool? Inherited { get; set; }
        public bool? RequiredValue { get; set; }
        public int? Order { get; set; }
        public int? InfoId { get; set; }
        public InfoBasic? Info { get; set; }
        public int? InfoTypeId { get; set; }
        public InfoTypeBasic? InfoType { get; set; }
        public int? MeasurementUnitId { get; set; }
        public object? MeasurementUnit { get; set; }
        public string? DisplayValue { get; set; }
        public bool? ForceScale { get; set; }
        public bool? ForceSignifDigits { get; set; }
        public string? ValueText { get; set; }
        public int? ValueInteger { get; set; }
        public double? ValueFloat { get; set; }
        public DateTime? ValueDateTime { get; set; }
        public bool? ValueBoolean { get; set; }
        public int? ValueAccountId { get; set; }
        public object? ValueAccount { get; set; }
        public int? DependentInfoId { get; set; }
        public object? DependentInfo { get; set; }
    }
}