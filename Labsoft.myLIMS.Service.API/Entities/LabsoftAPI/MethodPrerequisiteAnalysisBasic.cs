namespace LabsoftAPI
{
    public class MethodPrerequisiteAnalysisBasic {
        public int? Id { get; set; }
        public MethodBasic? Method { get; set; }
        public int? MethodMasterId { get; set; }
        public int? MethodId { get; set; }
        public InfoBasic? Info { get; set; }
        public int? InfoId { get; set; }
        public MethodStatus? MethodStatus { get; set; }
        public int? MethodStatusId { get; set; }
        public bool? AutomaticInsert { get; set; }
        public LastVersionMethod? LastVersionMethod { get; set; }
    }

    public class LastVersionMethod
    {
        public int? MasterId { get; set; }
        public int? Version { get; set; }
        public bool? LastVersion { get; set; }
        public List<int>? QCRoutineBatchIds { get; set; }
        public bool? Active { get; set; }
        public object? Duration { get; set; }
        public bool? AvailableSchedule { get; set; }
        public object? Master { get; set; }
        public object? MethodType { get; set; }
        public object? CalcEngineType { get; set; }
        public int? Id { get; set; }
        public string? Identification { get; set; }
    }

    public class MethodBasic
    {
        public object? EditionUser { get; set; }
        public object? EditionDateTime { get; set; }
        public object? ValidationUser { get; set; }
        public object? ValidationDateTime { get; set; }
        public object? Duration { get; set; }
        public int? MasterId { get; set; }
        public int? Version { get; set; }
        public bool? LastVersion { get; set; }
        public object? QCRoutineBatchIds { get; set; }
        public bool? Active { get; set; }
        public bool? AvailableSchedule { get; set; }
        public object? Master { get; set; }
        public object? MethodType { get; set; }
        public object? CalcEngineType { get; set; }
        public int? Id { get; set; }
        public string? Identification { get; set; }
    }
}