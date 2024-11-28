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

    public class LicenseGroup
    {
        public int Id { get; set; }
        public string? Identification { get; set; }
        public int UserReadOnlyAccessCount { get; set; }
        public int UserEditableAccessCount { get; set; }
    }
}