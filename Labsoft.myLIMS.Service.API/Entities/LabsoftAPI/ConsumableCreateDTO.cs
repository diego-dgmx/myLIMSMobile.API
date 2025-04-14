namespace LabsoftAPI
{

    public class ConsumableInfoCreateDTO
    {
        public int? InfoId { get; set; }
        public int? InfoTypeId { get; set; }
        public int? MeasurementUnitId { get; set; }
        public bool? ForceScale { get; set; }
        public bool? ForceSignifDigits { get; set; }
        public string? ValueText { get; set; }
        public int? ValueInteger { get; set; }
        public double? ValueFloat { get; set; }
        public DateTime? ValueDateTime { get; set; }
        public bool? ValueBoolean { get; set; }
        public int? ValueEquipmentId { get; set; }
        public int? ValueAccountId { get; set; }
        public int? ValueConsumableMovementId { get; set; }
        public int? ValueFileId { get; set; }
        public string? DisplayValue { get; set; }
        public bool? RequiredValue { get; set; }
        public bool? Inherited { get; set; }
        public int? DependentInfoId { get; set; }
        public int? Order { get; set; }
    }

    public class ConsumableServiceAreaCreateDTO
    {
        public int? ServiceAreaId { get; set; }
    }

    public class ConsumableServiceCenterCreateDTO
    {
        public int? ServiceCenterId { get; set; }
    }

    public class ConsumableCreateDTO
    {
        public string? Identification { get; set; }
        public string? ReferenceKey { get; set; }
        public DateTime? Expires { get; set; }
        public int? ExpireDaysAfterOpen { get; set; }
        public double? Quantity { get; set; }
        public double? Cost { get; set; }
        public int? ConsumableTypeId { get; set; }
        public int? HasExpires { get; set; }
        public DateTime? ExpireAfterOpen { get; set; }
        public bool? UseExpireAfterOpen { get; set; }
        public bool? UnrestrictedAccessServiceArea { get; set; }
        public bool? UnrestrictedAccessServiceCenter { get; set; }
        public DateTime? ExchangeDate { get; set; }
        public int? MultiCurrencyConfigCurrencyId { get; set; }
        public int? SampleId { get; set; }
        public List<ConsumableInfoCreateDTO>? ConsumableInfos { get; set; }
        public List<ConsumableServiceAreaCreateDTO>? ConsumableServiceAreas { get; set; }
        public List<ConsumableServiceCenterCreateDTO>? ConsumableServiceCenters { get; set; }
    }
}