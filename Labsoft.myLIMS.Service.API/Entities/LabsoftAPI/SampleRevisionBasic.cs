namespace LabsoftAPI {
    public class SampleRevisionChart
    {
        public int? FileId { get; set; }
    }

    public class SampleRevisionMessage
    {
        public int? Id { get; set; }
        public int? MessageHtmlFileId { get; set; }
        public string? Subject { get; set; }
        public string? Message { get; set; }
    }

    public class SampleRevisionBasic
    {
        public int? Id { get; set; }
        public int? SampleId { get; set; }
        public string? InfoIdentification { get; set; }
        public List<SampleRevisionMessage>? Messages { get; set; }
        public List<SampleRevisionChart>? Charts { get; set; }
        public bool? ObrigateNotConformityConclusionMessage { get; set; }
    }
}