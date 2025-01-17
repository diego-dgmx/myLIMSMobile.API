namespace LabsoftAPI {
    public class ConsumableTypeBasic
    {
        public int? Id { get; set; }
        public string? Identification { get; set; }
        public bool? Active { get; set; }
        public string? ReferenceKey { get; set; }
        public int? Precision { get; set; }
        public int? AdvanceNotice { get; set; }
        public int? MinQuantityNotice { get; set; }
        public MeasurementUnit? MeasurementUnit { get; set; }
        public bool? DoesntObligeQuantityStockMovement { get; set; }
    }

    public class MeasurementUnit
    {
        public int? Id { get; set; }
        public string? Identification { get; set; }
        public string? Description { get; set; }
    }
}