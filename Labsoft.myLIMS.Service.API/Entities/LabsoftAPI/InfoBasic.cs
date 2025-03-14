namespace LabsoftAPI {
    public class InfoBasic
    {
        public int? InfoTypeId { get; set; }
        public InfoTypeBasic? InfoType { get; set; }
        public bool? Active { get; set; }
        public object? MeasurementUnit { get; set; }
        public int? ForceScale { get; set; }
        public int? ForceSignifDigits { get; set; }
        public bool? ReadOnlyValue { get; set; }
        public int? EquipmentTypeId { get; set; }
        public int? AccountTypeId { get; set; }
        public bool? AllowAnyValue { get; set; }
        public bool? AllowText { get; set; }
        public int? ConsumableTypeId { get; set; }
        public ConsumableTypeBasic? ConsumableType { get; set; }
        public List<OptionBasic>? Options { get; set; }
        public int? DLLFileId { get; set; }
        public string? Custom01 { get; set; }
        public string? Custom02 { get; set; }
        public string? Custom03 { get; set; }
        public string? Custom04 { get; set; }
        public int? Id { get; set; }
        public string? Identification { get; set; }
    }

    public class OptionBasic
    {
        public int? Id { get; set; }
        public int? InfoId { get; set; }
        public InfoTypeBasic? InfoType { get; set; }
        public MeasurementUnitBasic? MeasurementUnit { get; set; }
        public bool? ForceScale { get; set; }
        public bool? ForceSignifDigits { get; set; }
        public string? DisplayValue { get; set; }
    }

    public class InfoTypeBasic
    {
        public int? Id { get; set; }
        public string? Identification { get; set; }
    }
}