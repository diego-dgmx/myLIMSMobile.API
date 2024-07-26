namespace LabsoftAPI {
    public class CalcEngineType
    {
        public int? Id { get; set; }
        public string? Identification { get; set; }
    }

    public class Master
    {
        public bool? Active { get; set; }
        public int? Id { get; set; }
        public string? Identification { get; set; }
    }

    public class MethodType
    {
        public int? Id { get; set; }
        public string? Identification { get; set; }
        public bool? Active { get; set; }
        public bool? RequiredMethodReferenceMethod { get; set; }
        public object? FlowStatus { get; set; }
    }

    public class AnalysisMethod
    {
        public int? MasterId { get; set; }
        public int? Version { get; set; }
        public bool? LastVersion { get; set; }
        public object? QCRoutineBatchIds { get; set; }
        public bool? Active { get; set; }
        public object? Duration { get; set; }
        public bool? AvailableSchedule { get; set; }
        public Master? Master { get; set; }
        public MethodType? MethodType { get; set; }
        public CalcEngineType? CalcEngineType { get; set; }
        public int? Id { get; set; }
        public string? Identification { get; set; }
    }
}