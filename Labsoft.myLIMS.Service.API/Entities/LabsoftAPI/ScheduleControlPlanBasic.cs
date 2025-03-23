namespace LabsoftAPI
{
    public class ScheduleControlPlanBasic
    {
        public int? ControlPlanId { get; set; }
        public ControlPlanBasic? ControlPlan { get; set; }
        public List<AnalysisDetailBasic>? AnalysisDetails { get; set; }
    }

    public class ControlPlanBasic
    {
        public int? Id { get; set; }
        public bool? Active { get; set; }
        public int? ActivationUserId { get; set; }
        public DateTime? ActivationDateTime { get; set; }
        public int? EditionUserId { get; set; }
        public DateTime? EditionDateTime { get; set; }
        public int? AnalysisGroupId { get; set; }
        public int? SpecificationId { get; set; }
        public AnalysisGroupBasic? AnalysisGroup { get; set; }
        public SpecificationBasic? Specification { get; set; }
    }

    public class AnalysisDetailBasic
    {
        public int? InfoId { get; set; }
        public InfoBasic? Info { get; set; }
        public int? MethodMasterId { get; set; }
        public MethodBasic? Method { get; set; }
        public int? MeasurementUnitId { get; set; }
        public MeasurementUnitBasic? MeasurementUnit { get; set; }
        public string? Attribute { get; set; }
        public int? UpperLimit { get; set; }
        public int? UpperLimitAttention { get; set; }
        public int? LowerLimitAttention { get; set; }
        public int? LowerLimit { get; set; }
        public string? TextLimit { get; set; }
        public string? DescriptionLimit { get; set; }
        public int? Order { get; set; }
        public string? Notes { get; set; }
        public string? Synonym { get; set; }
        public bool? Active { get; set; }
        public DateTime? EditionDateTime { get; set; }
    }
}
