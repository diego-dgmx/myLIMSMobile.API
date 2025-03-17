namespace LabsoftAPI {
    public class ConsumableServiceCenterBasic
    {
        public int? Id { get; set; }
        public ServiceCenterBasic? ServiceCenter { get; set; }
    }

    public class ServiceCenterBasic
    {
        public int? Id { get; set; }
        public string? Identification { get; set; }
        public bool? Active { get; set; }
        public PriceListBasic? PriceList { get; set; }
    }

    public class PriceListBasic
    {
        public int? Id { get; set; }
        public string? Identification { get; set; }
        public bool? Active { get; set; }
        public bool? ConsiderServiceCenterPriceListFromSampleOrWork { get; set; }
        public DateTime? Expire { get; set; }
    }
}