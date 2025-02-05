namespace LabsoftAPI
{
    public class ConsumableConsumptionInAnalysisDTO
    {
        public int? ConsumableId { get; set; }
        public int? Quantity { get; set; }
        public string? Notes { get; set; }
        public int? Sampleid { get; set; }
        public int? Methodid { get; set; }
        public int? Workid { get; set; }
        public int? ConsumableUsedId { get; set; }
        public List<int>? SampleContainerIds { get; set; }
        public int? Entity { get; set; }
        public int? EntityId { get; set; }
    }
}