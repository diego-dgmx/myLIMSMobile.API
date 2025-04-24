namespace LabsoftAPI
{
    public class PerformedTasksBasic
    {
        public string? MessageHTML { get; set; }
        public List<int>? FileDebugIds { get; set; }
        public bool? AllowRealize { get; set; }
        public List<AnalysisRevisionsToExecuteBasic>? AnalysisRevisionsToExecute { get; set; }
    }

    public class AnalysisRevisionsToExecuteBasic
    {
        public int? SampleId { get; set; }
        public int? SampleAnalysisId { get; set; }
        public int? AnalysisRevisionConfigId { get; set; }
        public string? Identification { get; set; }
        public bool? Debug { get; set; }
    }
}