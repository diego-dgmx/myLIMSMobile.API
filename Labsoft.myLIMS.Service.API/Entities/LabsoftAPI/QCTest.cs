namespace LabsoftAPI {
    public class QCRoutine
    {
        public int? Id { get; set; }
        public string? Identification { get; set; }
        public bool? UnrestrictedAccessServiceArea { get; set; }
        public bool? UnrestrictedAccessServiceCenter { get; set; }
        public bool? Active { get; set; }
    }

    public class QCRoutineBatch
    {
        public int? Id { get; set; }
        public string? Identification { get; set; }
        public int? TaskCountLimit { get; set; }
        public dynamic? Expires { get; set; }
        public bool? Active { get; set; }
        public EquipmentType? EquipmentType { get; set; }
        public QCRoutine? QCRoutine { get; set; }
    }

    public class QCTest
    {
        public int? Id { get; set; }
        public string? Identification { get; set; }
        public int? Number { get; set; }
        public int? Year { get; set; }
        public string? ControlNumber { get; set; }
        public int? TaskCountLimit { get; set; }
        public int? TaskCount { get; set; }
        public DateTime? Started { get; set; }
        public DateTime? Expires { get; set; }
        public object? EditionUserId { get; set; }
        public object? User { get; set; }
        public int? QCRoutineBatchId { get; set; }
        public QCRoutineBatch? QCRoutineBatch { get; set; }
        public int? EquipmentId { get; set; }
        public Equipment? Equipment { get; set; }
        public bool? AllRequiredControlSamplesPublished { get; set; }
    }
}