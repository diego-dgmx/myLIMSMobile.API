namespace LabsoftAPI
{
    public class MethodBackgroundParam
    {
        public int? Id { get; set; }
        public int? SampleId { get; set; }
        public int? MethodId { get; set; }
    }

    public class PerformTaskBackgroundParamsDTO
    {
        public bool? Advance { get; set; }
        public bool? Debug { get; set; }
        public bool? Calculate { get; set; }
        public bool? ExecuteAnalysisRevisions { get; set; }
        public int? VisualSpreadsheetId { get; set; }
        public List<TasksNextStatus>? TasksNextStatus { get; set; }
        public List<MethodBackgroundParam>? Methods { get; set; }
        public bool? ShowPopup { get; set; }
    }

    public class TasksNextStatus
    {
        public int? SampleId { get; set; }
        public int? MethodId { get; set; }
        public int? NextStatusId { get; set; }
    }
}