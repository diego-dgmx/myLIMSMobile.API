namespace LabsoftAPI {
    public class ConsumableUsed
    {
        public int? Id { get; set; }
        public string? Identification { get; set; }
        public bool? Active { get; set; }
        public string? ReferenceKey { get; set; }
        public DateTime? Expires { get; set; }
        public int? ExpireDaysAfterOpen { get; set; }
        public double? Quantity { get; set; }
        public double? Cost { get; set; }
        public DateTime? ExpireAfterOpen { get; set; }
        public bool? UseExpireAfterOpen { get; set; }
        public int? SampleId { get; set; }
        public int? ConsumableTypeId { get; set; }
        public ConsumableTypeBasic? ConsumableType { get; set; }
    }

    public class SingleData
    {
        public int? Id { get; set; }
        public string? Identification { get; set; }
    }

    public class ConsumableMovementBasic
    {
        public int? Id { get; set; }
        public int? MovementTypeId { get; set; }
        public SingleData? MovementType { get; set; }
        public double? Quantity { get; set; }
        public string? Notes { get; set; }
        public bool? Active { get; set; }
        public int? SampleId { get; set; }
        public SingleInfoData? Sample { get; set; }
        public int? MethodId { get; set; }
        public SingleData? Method { get; set; }
        public int? WorkId { get; set; }
        public SingleInfoData? Work { get; set; }
        public int? ConsumableUsedId { get; set; }
        public ConsumableUsed? ConsumableUsed { get; set; }
        public int? EditionUserId { get; set; }
        public SingleData? EditionUser { get; set; }
        public DateTime? EditionDateTime { get; set; }
        public string? DeactiveNotes { get; set; }
        public int? ContainerId { get; set; }
    }

    public class SingleInfoData
    {
        public int? Id { get; set; }
        public string? Identification { get; set; }
        public string? ControlNumber { get; set; }
        public bool? Active { get; set; }
    }
}