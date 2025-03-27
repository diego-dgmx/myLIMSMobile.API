namespace LabsoftAPI
{
    public class ScheduleAnalysisGroupBasic
    {
        public int? Id { get; set; }
        public int? AnalysisGroupId { get; set; }
        public AnalysisGroupBasic? AnalysisGroup { get; set; }
    }

    public partial class AnalysisGroupBasic
    {
        public int? Id { get; set; }
        public string? Identification { get; set; }
    }
}
