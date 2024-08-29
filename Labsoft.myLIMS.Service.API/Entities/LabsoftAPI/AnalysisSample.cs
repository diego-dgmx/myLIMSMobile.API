namespace LabsoftAPI {
    public class Account
    {
        public object? Complement { get; set; }
        public object? ReferenceKey { get; set; }
        public AccountType? AccountType { get; set; }
        public object? PriceList { get; set; }
        public bool? Active { get; set; }
        public bool? RelatedAccountRequired { get; set; }
        public object? RegistryNumber { get; set; }
        public object? CultureId { get; set; }
        public int? Id { get; set; }
        public string? Identification { get; set; }
    }

    public class AccountType
    {
        public bool? Active { get; set; }
        public bool? RequireRegistryNumber { get; set; }
        public object? AccountTypeRegistryTypeId { get; set; }
        public int? Id { get; set; }
        public string? Identification { get; set; }
    }

    public class CurrentStatus
    {
        public int? Id { get; set; }
        public SampleStatus? SampleStatus { get; set; }
        public EditionUser? EditionUser { get; set; }
        public DateTime? EditionDateTime { get; set; }
        public MethodStatus? MethodStatus { get; set; }
        public bool? IsRework { get; set; }
        public bool? InProcess { get; set; }
        public object? ExecuteUser { get; set; }
        public DateTime? ExecuteDateTime { get; set; }
        public StartUser? StartUser { get; set; }
        public DateTime? StartDateTime { get; set; }
    }

    public class EditionUser
    {
        public bool? Active { get; set; }
        public object? CultureId { get; set; }
        public object? Login { get; set; }
        public object? Email { get; set; }
        public object? ServiceCenter { get; set; }
        public object? ServiceArea { get; set; }
        public object? Account { get; set; }
        public object? SignatureCertFileId { get; set; }
        public object? LicenseGroup { get; set; }
        public object? ReadOnlyAccess { get; set; }
        public object? Reserved { get; set; }
        public object? ServiceAreaId { get; set; }
        public object? ExternalUser { get; set; }
        public int? Id { get; set; }
        public string? Identification { get; set; }
    }

    public class MethodStatus
    {
        public bool? Active { get; set; }
        public int? MethodStatusBehaviorId { get; set; }
        public object? MethodStatusBehavior { get; set; }
        public int? Id { get; set; }
        public string? Identification { get; set; }
    }

    public class PriceList
    {
        public bool? Active { get; set; }
        public object? Expire { get; set; }
        public bool? ConsiderServiceCenterPriceListFromSampleOrWork { get; set; }
        public int? Id { get; set; }
        public string? Identification { get; set; }
    }

    public class QCTests
    {
        public int? Id { get; set; }
        public required QCTest QCTest { get; set; }
        public object? SampleMethod { get; set; }
    }

    public class QCTest
    {
        public int? Id { get; set; }
        public int? Number { get; set; }
        public int? Year { get; set; }
        public string? ControlNumber { get; set; }
        public object? TaskCountLimit { get; set; }
        public object? TaskCount { get; set; }
        public int? QCRoutineBatchId { get; set; }
        public object? QCRoutineBatch { get; set; }
        public object? Equipment { get; set; }
        public object? Started { get; set; }
        public object? Expires { get; set; }
        public bool? AllRequiredControlSamplesPublished { get; set; }
    }

    public class AnalysisSample
    {
        public List<QCTests>? QCTests { get; set; }
        public List<object>? PrerequisiteAnalyses { get; set; }
        public int? AnalysisDeadlineEmpty { get; set; }
        public int? ConclusionEmpty { get; set; }
        public SampleCustomInfo? SampleCustomInfo { get; set; }
        public int? Id { get; set; }
        public DateTime? AnalysisDeadline { get; set; }
        public DateTime? Conclusion { get; set; }
        public Sample? Sample { get; set; }
        public AnalysisMethod? Method { get; set; }
        public ServiceArea? ServiceArea { get; set; }
        public CurrentStatus? CurrentStatus { get; set; }
    }

    public class Sample
    {
        public int? Id { get; set; }
        public string? Identification { get; set; }
        public object? ControlIdentification { get; set; }
        public string? ControlNumber { get; set; }
        public object? ReferenceKey { get; set; }
        public object? Prefix { get; set; }
        public int? GroupId { get; set; }
        public int? Number { get; set; }
        public int? Year { get; set; }
        public int? SubNumber { get; set; }
        public int? Revision { get; set; }
        public bool? Active { get; set; }
        public bool? SyncPortal { get; set; }
        public bool? Received { get; set; }
        public bool? Finalized { get; set; }
        public bool? Published { get; set; }
        public bool? Reviewed { get; set; }
        public DateTime? Conclusion { get; set; }
        public DateTime? TakenDateTime { get; set; }
        public DateTime? ReceivedTime { get; set; }
        public object? FinalizedTime { get; set; }
        public object? PublishedTime { get; set; }
        public object? ReviewedTime { get; set; }
        public object? ExpectedCollectionTime { get; set; }
        public object? ReferenceSampleId { get; set; }
        public object? ReferenceSample { get; set; }
        public object? ConclusionTime { get; set; }
        public bool? ConclusionTimeFixed { get; set; }
        public List<object>? QCTestsControlSample { get; set; }
        public ServiceCenter? ServiceCenter { get; set; }
        public object? SampleConclusion { get; set; }
        public PriceList? PriceList { get; set; }
        public SampleReason? SampleReason { get; set; }
        public CurrentStatus? CurrentStatus { get; set; }
        public SampleType? SampleType { get; set; }
        public object? CollectionPointId { get; set; }
        public object? CollectionPoint { get; set; }
        public Account? Account { get; set; }
        public object? RelatedAccount { get; set; }
        public List<SampleWork>? SampleWorks { get; set; }
        public object? CustomInfo { get; set; }
        public object? TotalPrice { get; set; }
        public object? TotalPriceBusinessUnit { get; set; }
    }

    public class SampleReason
    {
        public bool? Active { get; set; }
        public int? Id { get; set; }
        public string? Identification { get; set; }
    }

    public class SampleStatus
    {
        public int? Id { get; set; }
        public string? Identification { get; set; }
        public bool? BeforeReceive { get; set; }
        public bool? AfterPublish { get; set; }
        public bool? PortalSampleStatus { get; set; }
    }

    public class SampleType
    {
        public object? Prefix { get; set; }
        public bool? Active { get; set; }
        public object? SampleClassId { get; set; }
        public object? SampleClass { get; set; }
        public object? SamplePublishTypeId { get; set; }
        public object? SamplePublishType { get; set; }
        public object? SampleReasonId { get; set; }
        public object? SampleReason { get; set; }
        public object? SampleTypeParentId { get; set; }
        public object? SampleTypeParent { get; set; }
        public object? ReferenceKey { get; set; }
        public bool? PackagingAlert { get; set; }
        public int? Id { get; set; }
        public string? Identification { get; set; }
    }

    public class SampleWork
    {
        public int? Id { get; set; }
        public Work? Work { get; set; }
    }

    public class ServiceArea
    {
        public ServiceCenter? ServiceCenter { get; set; }
        public object? ExtraTime { get; set; }
        public bool? ExternalServiceArea { get; set; }
        public bool? Active { get; set; }
        public int? Id { get; set; }
        public string? Identification { get; set; }
    }

    public class ServiceCenter
    {
        public bool? Active { get; set; }
        public object? PriceList { get; set; }
        public int? Id { get; set; }
        public string? Identification { get; set; }
    }

    public class StartUser
    {
        public bool? Active { get; set; }
        public object? CultureId { get; set; }
        public object? Login { get; set; }
        public object? Email { get; set; }
        public object? ServiceCenter { get; set; }
        public object? ServiceArea { get; set; }
        public object? Account { get; set; }
        public object? SignatureCertFileId { get; set; }
        public object? LicenseGroup { get; set; }
        public object? ReadOnlyAccess { get; set; }
        public object? Reserved { get; set; }
        public object? ServiceAreaId { get; set; }
        public object? ExternalUser { get; set; }
        public int? Id { get; set; }
        public string? Identification { get; set; }
    }

    public class Work
    {
        public int? Id { get; set; }
        public string? ControlNumber { get; set; }
        public int? Number { get; set; }
        public int? Year { get; set; }
        public object? Identification { get; set; }
        public bool? Active { get; set; }
        public object? ReferenceKey { get; set; }
        public bool? Confidential { get; set; }
        public bool? Finished { get; set; }
        public object? WorkType { get; set; }
        public object? Account { get; set; }
        public object? RelatedAccount { get; set; }
        public object? OwnerUser { get; set; }
        public object? CurrentWorkFlow { get; set; }
        public object? Priority { get; set; }
        public object? ServiceCenter { get; set; }
        public object? WorkConclusion { get; set; }
        public object? WorkClass { get; set; }
        public object? WorkSubClass { get; set; }
    }

    public class SampleCustomInfo {
        public string? DisplayValue { get; set; }
        public int InfoTypeId { get; set; }
        public string? ForceScale { get; set; }
        public string? ForceSignifDigits { get; set; }
        public string? ValueDateTime { get; set; }
    }
}