namespace Entities
{
    public class SampleDTO {
        public int? Id { get; set; }
        public string? Identification { get; set; }
        public DateTime? Conclusion { get; set; }
        public DateTime? TakenDateTime { get; set; }
        public DateTime? ReceivedTime { get; set; }
        public CurrentStatus? CurrentStatus { get; set; }
        public SampleServiceArea? ServiceArea { get; set; }
        public SampleType? SampleType { get; set; }
        public Method? Method { get; set; }
    }

    public class CurrentStatus
    {
        public int? Id { get; set; }
        public SampleStatus? SampleStatus { get; set; }
    }

    public class SampleStatus
    {
        public int? Id { get; set; }
        public string? Identification { get; set; }
        public bool? BeforeReceive { get; set; }
        public bool? AfterPublish { get; set; }
        public bool? PortalSampleStatus { get; set; }
    }

    public class SampleServiceArea
    {
        public object? ExtraTime { get; set; }
        public bool? ExternalServiceArea { get; set; }
        public bool? Active { get; set; }
        public int? Id { get; set; }
        public string? Identification { get; set; }
    }

    public class SampleType
    {
        public int? Id { get; set; }
        public string? Identification { get; set; }
    }

    public class Method
    {
        public int? MasterId { get; set; }
        public int? Id { get; set; }
        public string? Identification { get; set; }
    }
}