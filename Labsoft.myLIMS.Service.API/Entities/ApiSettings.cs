namespace Entities
{
    public class ApiSettings
    {
        public required string LabsoftAuthURL { get; set; }
        public required string LabsoftIdentityCenterApiURLBase { get; set; }
        public required string MyLIMSApiURLBase { get; set; }
        public required string MyLIMSApiAccessKey { get; set; }
        public required string LabsoftAuthClientId { get; set; }
        public required string LabsoftAuthScope { get; set; }
    }
}