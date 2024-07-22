using System.Text.Json.Serialization;

namespace Labsoft.myLIMS.Service.API.Entities
{
    public class RootObject
    {
        [JsonPropertyName("result")]  // Assuming JSON has "result" instead of "Users"
        public List<User> Users { get; set; }
    }

    public class User
    {
        public bool Active { get; set; }
        public string CultureId { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public ServiceCenter ServiceCenter { get; set; }
        public ServiceArea ServiceArea { get; set; }
        public Account Account { get; set; }
        public int? SignatureCertFileId { get; set; }
        public LicenseGroup LicenseGroup { get; set; }
        public bool? ReadOnlyAccess { get; set; }
        public int? ServiceAreaId { get; set; }
        public int Id { get; set; }
        public string Identification { get; set; }
    }
}

public class ServiceCenter
{
    public bool Active { get; set; }
    public int Id { get; set; }
    public string Identification { get; set; }
}

public class ServiceArea
{
    public ServiceCenter ServiceCenter { get; set; }
    public bool Active { get; set; }
    public int Id { get; set; }
    public string Identification { get; set; }
}

public class Account
{
    public int Id { get; set; }
    public string Identification { get; set; }
    public bool Active { get; set; }
}

public class LicenseGroup
{
    public int Id { get; set; }
    public string Identification { get; set; }
    public int UserReadOnlyAccessCount { get; set; }
    public int UserEditableAccessCount { get; set; }
}
