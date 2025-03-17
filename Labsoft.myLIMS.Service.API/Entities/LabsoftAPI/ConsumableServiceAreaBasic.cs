namespace LabsoftAPI {
    public class ConsumableServiceAreaBasic
    {
        public int? Id { get; set; }
        public ServiceAreaBasic? ServiceArea { get; set; }
    }

    public class ServiceAreaBasic
    {
        public int? Id { get; set; }
        public string? Identification { get; set; }
        public bool? Active { get; set; }
        public int? ServiceCenterId { get; set; }
        public int? ExtraTime { get; set; }
        public bool? ExternalServiceArea { get; set; }
        public bool? AvailableForWork { get; set; }
        public int? ActivationUserId { get; set; }
        public SingleData? ActivationUser { get; set; }
        public DateTime? ActivationDateTime { get; set; }
        public int? EditionUserId { get; set; }
        public SingleData? EditionUser { get; set; }
        public DateTime? EditionDateTime { get; set; }
        public ServiceCenterBasic? ServiceCenter { get; set; }
    }
}