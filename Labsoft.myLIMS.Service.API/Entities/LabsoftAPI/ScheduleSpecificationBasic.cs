namespace LabsoftAPI
{
    public class ScheduleSpecificationBasic
    {
        public int? Id { get; set; }
        public int? SpecificationId { get; set; }
        public SpecificationBasic? Specification { get; set; }
    }

    public partial class SpecificationBasic
    {
        public int? SId { get; set; }
        public bool? Active { get; set; }
        public int? EditionUserId { get; set; }
        public DateTime? EditionDateTime { get; set; }
        public string? NotConformedMessage { get; set; }
        public string? AttentionMessage { get; set; }
        public string? ConformedMessage { get; set; }
        public string? Description { get; set; }
        public int? IdAux { get; set; }
        public int? Version { get; set; }
        public bool? LastVersion { get; set; }
        public int? ValidationUserId { get; set; }
        public DateTime? ValidationDateTime { get; set; }
        public int? ActivationUserId { get; set; }
        public DateTime? ActivationDateTime { get; set; }
        public bool? ControlPlan { get; set; }
        public bool? UnrestrictedAccessServiceCenter { get; set; }
        public bool? UnrestrictedAccessServiceArea { get; set; }
        public int? Id { get; set; }
        public string? Identification { get; set; }
    }
}
