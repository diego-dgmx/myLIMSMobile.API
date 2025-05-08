namespace LabsoftAPI
{
    public class ScheduleInterventionAnalysisGroupDTO
    {
        public int? Id { get; set; }
        public int? SId { get; set; }
        public int? ScheduleInterventionId { get; set; }
        public int? AnalysisGroupId { get; set; }
        public int? EditionUserId { get; set; }
        public DateTime? EditionDateTime { get; set; }
    }

    public class ScheduleInterventionControlPlanAnalysisDTO
    {
        public int? Id { get; set; }
        public int? SId { get; set; }
        public int? ControlPlanId { get; set; }
        public int? Order { get; set; }
        public int? InfoId { get; set; }
        public int? MethodMasterId { get; set; }
        public int? MeasurementUnitId { get; set; }
        public string? Attribute { get; set; }
        public int? UpperLimit { get; set; }
        public int? UpperLimitAttention { get; set; }
        public int? LowerLimitAttention { get; set; }
        public int? LowerLimit { get; set; }
        public string? TextLimit { get; set; }
        public string? DescriptionLimit { get; set; }
        public string? Notes { get; set; }
        public int? EditionUserId { get; set; }
        public DateTime? EditionDateTime { get; set; }
        public string? Synonym { get; set; }
    }

    public class ScheduleInterventionCreateDTO
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
        public List<ScheduleInterventionAnalysisGroupDTO>? AnalysisGroups { get; set; }
        public List<ScheduleInterventionSpecificationsDTO>? Specifications { get; set; }
        public List<ScheduleInterventionControlPlanAnalysisDTO>? ControlPlanAnalysis { get; set; }
    }

    public class ScheduleInterventionSpecificationsDTO
    {
        public int? Id { get; set; }
        public int? SId { get; set; }
        public int? ScheduleInterventionId { get; set; }
        public int? SpecificationId { get; set; }
        public int? EditionUserId { get; set; }
        public DateTime? EditionDateTime { get; set; }
    }
}