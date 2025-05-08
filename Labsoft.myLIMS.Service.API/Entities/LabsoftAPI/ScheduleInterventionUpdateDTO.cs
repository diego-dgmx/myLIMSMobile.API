namespace LabsoftAPI
{
    public class ScheduleInterventionAnalysisGroupsUpdateDTO
    {
        public List<ScheduleInterventionAnalysisGroupDTO>? Inserted { get; set; }
        public List<ScheduleInterventionAnalysisGroupDTO>? Modified { get; set; }
        public List<ScheduleInterventionAnalysisGroupDTO>? Removed { get; set; }
    }

    public class ScheduleInterventionControlPlanAnalysisUpdateDTO
    {
        public List<ScheduleInterventionControlPlanAnalysisDTO>? Inserted { get; set; }
        public List<ScheduleInterventionControlPlanAnalysisDTO>? Modified { get; set; }
        public List<ScheduleInterventionControlPlanAnalysisDTO>? Removed { get; set; }
    }

    public class ScheduleInterventionUpdateDTO
    {
        public int? Id { get; set; }
        public int? EquipmentId { get; set; }
        public bool? Active { get; set; }
        public int? Interval { get; set; }
        public DateTime? LastIntervention { get; set; }
        public DateTime? NextIntervention { get; set; }
        public int? EditionUserId { get; set; }
        public DateTime? EditionDateTime { get; set; }
        public int? AdvanceNotice { get; set; }
        public string? Identification { get; set; }
        public int? InterventionTypeId { get; set; }
        public int? ActivationUserId { get; set; }
        public DateTime? ActivationDateTime { get; set; }
        public bool? AvailableOffline { get; set; }
        public bool? AutomaticCreation { get; set; }
        public bool? Antecedence { get; set; }
        public ScheduleInterventionAnalysisGroupsUpdateDTO? AnalysisGroups { get; set; }
        public ScheduleInterventionSpecificationsUpdateDTO? Specifications { get; set; }
        public ScheduleInterventionControlPlanAnalysisUpdateDTO? ControlPlanAnalysis { get; set; }
    }

    public class ScheduleInterventionSpecificationsUpdateDTO
    {
        public List<ScheduleInterventionSpecificationsDTO>? Inserted { get; set; }
        public List<ScheduleInterventionSpecificationsDTO>? Modified { get; set; }
        public List<ScheduleInterventionSpecificationsDTO>? Removed { get; set; }
    }
}