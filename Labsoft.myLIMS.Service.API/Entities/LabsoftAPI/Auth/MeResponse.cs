namespace LabsoftAPI.Auth
{
    public class MeResponse
    {
        public bool Active { get; set; }
        public string? CultureId { get; set; }
        public string? Login { get; set; }
        public string? Email { get; set; }
        public ServiceCenter? ServiceCenter { get; set; }
        public ServiceArea? ServiceArea { get; set; }
        public Account? Account { get; set; }
        public object? SignatureCertFileId { get; set; }
        public LicenseGroup? LicenseGroup { get; set; }
        public bool ReadOnlyAccess { get; set; }
        public object? Reserved { get; set; }
        public int ServiceAreaId { get; set; }
        public object? ExternalUser { get; set; }
        public int Id { get; set; }
        public string? Identification { get; set; }
    }

    public class Account
    {
        public object? Complement { get; set; }
        public object? ReferenceKey { get; set; }
        public object? AccountType { get; set; }
        public object? PriceList { get; set; }
        public bool Active { get; set; }
        public bool RelatedAccountRequired { get; set; }
        public object? RegistryNumber { get; set; }
        public object? CultureId { get; set; }
        public int Id { get; set; }
        public string? Identification { get; set; }
    }

    public class LicenseGroup
    {
        public int Id { get; set; }
        public string? Identification { get; set; }
        public int UserReadOnlyAccessCount { get; set; }
        public int UserEditableAccessCount { get; set; }
    }

    public class ServiceArea
    {
        public object? ServiceCenter { get; set; }
        public object? ExtraTime { get; set; }
        public bool ExternalServiceArea { get; set; }
        public bool Active { get; set; }
        public int Id { get; set; }
        public string? Identification { get; set; }
    }

    public class ServiceCenter
    {
        public bool Active { get; set; }
        public object? PriceList { get; set; }
        public int Id { get; set; }
        public string? Identification { get; set; }
    }
}