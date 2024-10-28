namespace LabsoftAPI {
    public class QCTestLink
    {
        public int Id { get; set; }
        public QCTestSample? Sample { get; set; }
    }

    public class QCTestSample
    {
        public int? Id { get; set; }
        public string? ControlNumber { get; set; }
        public string? Identification { get; set; }
        public bool? Active { get; set; }
        public int? SampleTypeId { get; set; }
        public string? SampleTypeIdentification { get; set; }
        public object? CollectionPointId { get; set; }
        public object? CollectionPointIdentification { get; set; }
        public int? SampleCurrentStatusId { get; set; }
        public int? SampleStatusId { get; set; }
        public string? SampleStatusIdentification { get; set; }
        public int? SampleConclusionId { get; set; }
        public string? SampleConclusionIdentification { get; set; }
    }
}
