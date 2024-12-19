namespace LabsoftAPI {
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ActivationUser
    {
        public int? Id { get; set; }
        public string? Identification { get; set; }
    }

    public class SimpleEditionUser
    {
        public int? Id { get; set; }
        public string? Identification { get; set; }
    }

    public class ConsumableBasic
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
        public string? SampleConclusion { get; set; }
        public SimpleEditionUser? EditionUser { get; set; }
        public DateTime? EditionDateTime { get; set; }
        public ActivationUser? ActivationUser { get; set; }
        public DateTime? ActivationDateTime { get; set; }
        public StatusUser? StatusUser { get; set; }
        public DateTime? StatusDateTime { get; set; }
    }

    public class StatusUser
    {
        public int? Id { get; set; }
        public string? Identification { get; set; }
    }
}