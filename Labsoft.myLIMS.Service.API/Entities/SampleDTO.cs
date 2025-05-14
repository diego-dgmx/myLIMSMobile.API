using LabsoftAPI;

namespace Entities
{
    public class SampleDTO {
        public int? Id { get; set; }
        public int? SampleMethodId { get; set; }
        public string? Identification { get; set; }
        public DateTime? Conclusion { get; set; }
        public DateTime? TakenDateTime { get; set; }
        public DateTime? ReceivedTime { get; set; }
        public List<QCTests>? QCTests { get; set; }
        public List<PrerequisiteAnalysesBasic>? PrerequisiteAnalyses { get; set; }
        public CurrentStatus? CurrentStatus { get; set; }
        public SampleServiceArea? ServiceArea { get; set; }
        public SampleType? SampleType { get; set; }
        public Method? Method { get; set; }
    }

    public class SampleServiceArea
    {
        public object? ExtraTime { get; set; }
        public bool? ExternalServiceArea { get; set; }
        public bool? Active { get; set; }
        public int? Id { get; set; }
        public string? Identification { get; set; }
    }

    public class BatchQC
    {
        public int? Id { get; set; }
        public string? Identification { get; set; }
    }

    public class SampleNumber {
        public int? Id { get; set; }
        public string? Identification { get; set; }
    }

    public class CollectionPoint {
        public int? Id { get; set; }
        public string? Identification { get; set; }
    }

    public class SampleActivity{
        public int? Id { get; set; }
        public string? Identification { get; set; }
    }
}