namespace LabsoftAPI.Auth
{
    public class UserDataBasic
    {
        public int? Id { get; set; }
        public List<UserProfilesBasic>? UserProfiles { get; set; }
    }

    public class EntitiesActionBasic
    {
        public int? Id { get; set; }
        public int? SId { get; set; }
        public int? UserProfileId { get; set; }
        public int? EntityActionId { get; set; }
        public EntityActionBasic? EntityAction { get; set; }
    }

    public class EntityActionBasic
    {
        public int? Id { get; set; }
        public int? SId { get; set; }
        public string? Entity { get; set; }
        public string? Action { get; set; }
        public string? Tag { get; set; }
        public bool? ReadOnlyAccess { get; set; }
    }

    public class UserProfilesBasic
    {
        public int? Id { get; set; }
        public object? SId { get; set; }
        public int? UserId { get; set; }
        public int? UserProfileId { get; set; }
        public UserProfileBasic? UserProfile { get; set; }
        public int? EditionUserId { get; set; }
        public object? EditionUser { get; set; }
        public object? Expire { get; set; }
        public string? EditionView { get; set; }
        public DateTime? EditionDateTime { get; set; }
    }

    public class UserProfileBasic
    {
        public int? Id { get; set; }
        public int? SId { get; set; }
        public string? Identification { get; set; }
        public bool? UnrestrictedAccess { get; set; }
        public bool? UnrestrictedAccessServiceArea { get; set; }
        public bool? UnrestrictedAccessMessageType { get; set; }
        public bool? UnrestrictedAccessWorkType { get; set; }
        public bool? UnrestrictedAccessReport { get; set; }
        public bool? UnrestrictedAccessDataViewer { get; set; }
        public bool? UnrestrictedAccessServiceCenter { get; set; }
        public bool? UnrestrictedAccessSampleType { get; set; }
        public bool? UnrestrictedAccessCollectionPoint { get; set; }
        public bool? UnrestrictedAccessSampleStatus { get; set; }
        public bool? UnrestrictedAccessAccount { get; set; }
        public bool? UnrestrictedAccessAccountType { get; set; }
        public bool? UnrestrictedAccessDashBoard { get; set; }
        public bool? UnrestrictedAccessDashBoardWork { get; set; }
        public bool? UnrestrictedAccessDashBoardAccount { get; set; }
        public bool? UnrestrictedAccessFileSystem { get; set; }
        public bool? UnrestrictedAccessMethod { get; set; }
        public bool? UnrestrictedAccessOperationScreen { get; set; }
        public bool? UnrestrictedAccessCustomModule { get; set; }
        public bool? UnrestrictedAccessBusinessUnit { get; set; }
        public bool? AllowRelatedAccountsAccess { get; set; }
        public bool? UnrestrictedAccessIPRange { get; set; }
        public int? EditionUserId { get; set; }
        public dynamic? EditionUser { get; set; }
        public DateTime? EditionDateTime { get; set; }
        public string? LogonPermissions { get; set; }
        public dynamic? MaximumDiscount { get; set; }
        public bool? UnrestrictedAccessPriceList { get; set; }
        public string? EditionView { get; set; }
        public List<EntitiesActionBasic>? EntitiesActions { get; set; }
        public dynamic? ServiceAreas { get; set; }
        public dynamic? MessageTypes { get; set; }
        public dynamic? OperationScreen { get; set; }
        public dynamic? WorkTypes { get; set; }
        public dynamic? Reports { get; set; }
        public dynamic? DataViewers { get; set; }
        public List<dynamic>? Users { get; set; }
        public dynamic? DashBoards { get; set; }
        public dynamic? ServiceCenters { get; set; }
        public dynamic? SampleTypes { get; set; }
        public dynamic? CollectionPoints { get; set; }
        public dynamic? SampleStatus { get; set; }
        public dynamic? Accounts { get; set; }
        public dynamic? AccountTypes { get; set; }
        public dynamic? Paths { get; set; }
        public dynamic? Methods { get; set; }
        public dynamic? IPRanges { get; set; }
        public dynamic? CustomModule { get; set; }
        public dynamic? PriceList { get; set; }
        public dynamic? BusinessUnits { get; set; }
    }
}