namespace LabsoftAPI {
    public class SimpleConsumableBasic
    {
        public int? Id { get; set; }
        public string? Identification { get; set; }
        public DateTime? Expires { get; set; }
        public int? ExpireDaysAfterOpen { get; set; }
        public double? Quantity { get; set; }
        public double? Cost { get; set; }
    }
}