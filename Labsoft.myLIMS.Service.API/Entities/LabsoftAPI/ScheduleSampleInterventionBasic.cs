namespace LabsoftAPI
{
    public class ScheduleSampleInterventionBasic
    {
        public int? SampleId { get; set; }
        public string? ControlNumber { get; set; }
        public string? Identification { get; set; }
        public string? SampleStatus { get; set; }
        public DateTime? EditionDateTime { get; set; }
        public string? SampleConclusion { get; set; }
        public string? SampleReason { get; set; }
        public int? ScheduleInterventionId { get; set; }
        public DateTime? SampleTakenDateTime { get; set; }
        public int? EquipmentId { get; set; }
        public Equipment? Equipment { get; set; }
        public int? InterventionTypeId { get; set; }
    }
}
