namespace LabsoftAPI
{
    public class ScheduleInterventionBasic
    {
        public int? Id { get; set; }
        public string? Identification { get; set; }
        public InterventionType? InterventionType { get; set; }
        public int? Interval { get; set; }
        public int? AdvanceNotice { get; set; }
        public DateTime? LastIntervention { get; set; }
        public DateTime? NextIntervention { get; set; }
        public bool? Active { get; set; }
        public Equipment? Equipment { get; set; }
        public bool? AvailableOffline { get; set; }
        public bool? AutomaticCreation { get; set; }
        public bool? Antecedence { get; set; }
    }

    public partial class InterventionType
    {
        public int? Id { get; set; }
        public string? Identification { get; set; }
        public bool? Active { get; set; }
    }
}
