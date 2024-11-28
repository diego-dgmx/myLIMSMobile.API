namespace LabsoftAPI {
    public class Equipment
    {
        public bool? Active { get; set; }
        public bool? AvailableSchedule { get; set; }
        public EquipmentType? EquipmentType { get; set; }
        public int? Id { get; set; }
        public string? Identification { get; set; }
    }

    public class EquipmentType
    {
        public bool? Active { get; set; }
        public int? Id { get; set; }
        public string? Identification { get; set; }
    }
}