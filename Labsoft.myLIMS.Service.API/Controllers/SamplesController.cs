using Entities;
using Interfaces;
using LabsoftAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Services;

namespace Labsoft.myLIMS.Service.API.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SamplesController(ISamplesServices samplesServices, ILogger<SamplesController> logger) : ControllerBase
    {
        private readonly ISamplesServices _samplesServices = samplesServices;
        private readonly ILogger<SamplesController> _logger = logger;
        
        [Flags]
        private enum SampleType
        {
            carriedOutAnalysis = 1,
            reviewAnalysis = 2,
            prepareAnalysis = 4
        }

        private enum SampleSortParam
        {
            batchQC,
            id,
            analyticsMethod,
            sampleType,
            stage,
            serviceArea,
            startUser,
            date,
            sampleNumber,
            sampleIdentification,
            customInfo,
            activities,
            collectionPoint,
            sampleReason
        }

        [HttpGet("GetAllMethods")]
        public async Task<ActionResult<ResponseBase<List<AnalysisMethod>>>> GetAllMethods()
        {
            var response = await _samplesServices.GetAllMethods();
            
            if(response.StatusCode == 200)
            {
                var results = response.Success ?? [];

                return StatusCode(response.StatusCode, new ResponseBase<List<AnalysisMethod>>
                {
                    Ok = response.Success != null,
                    Data = results
                });
            }
            else
            {
                LogConfiguration.CreateLogSender(HttpContext.Request, _logger, response.Error?.Exception);
                return StatusCode(response.StatusCode, new ResponseBase<dynamic>
                {
                    Ok = false,
                    Message = response.Error?.ErrorDescription,
                    Error = new ErrorBase
                    {
                        Code = response.Error?.Error,
                        Description = response.Error?.ErrorDescription
                    }
                });
            }
        }

        [HttpGet("GetAvailableStages")]
        public async Task<ActionResult<ResponseBase<List<MethodStatus>>>> GetAvailableStages()
        {
            var response = await _samplesServices.GetAllSamples();
            
            if(response.StatusCode == 200)
            {
                var results = response.Success?.Result ?? [];

                return StatusCode(response.StatusCode, new ResponseBase<List<MethodStatus>>
                {
                    Ok = response.Success != null,
                    Data = results.Select(sample => sample.CurrentStatus!.MethodStatus)
                        .DistinctBy(m => m!.Id).OrderBy(m => m?.Identification).ToList()!
                });
            }
            else
            {
                LogConfiguration.CreateLogSender(HttpContext.Request, _logger, response.Error?.Exception);
                return StatusCode(response.StatusCode, new ResponseBase<dynamic>
                {
                    Ok = false,
                    Message = response.Error?.ErrorDescription,
                    Error = new ErrorBase
                    {
                        Code = response.Error?.Error,
                        Description = response.Error?.ErrorDescription
                    }
                });
            }
        }

        [HttpGet("GetAvailableServiceAreas")]
        public async Task<ActionResult<ResponseBase<List<SampleServiceArea>>>> GetAvailableServiceAreas()
        {
            var response = await _samplesServices.GetAllSamples();
            
            if(response.StatusCode == 200)
            {
                var results = response.Success?.Result ?? [];

                return StatusCode(response.StatusCode, new ResponseBase<List<SampleServiceArea>>
                {
                    Ok = response.Success != null,
                    Data = results.Select(sample => sample.ServiceArea)
                        .DistinctBy(m => m?.Id)
                        .Select(m => new SampleServiceArea
                        {
                            Id = m?.Id,
                            ExtraTime = m?.ExtraTime,
                            ExternalServiceArea = m?.ExternalServiceArea,
                            Active = m?.Active,
                            Identification = m?.Identification
                        })
                        .OrderBy(m => m?.Identification).ToList()!
                });
            }
            else
            {
                LogConfiguration.CreateLogSender(HttpContext.Request, _logger, response.Error?.Exception);
                return StatusCode(response.StatusCode, new ResponseBase<dynamic>
                {
                    Ok = false,
                    Message = response.Error?.ErrorDescription,
                    Error = new ErrorBase
                    {
                        Code = response.Error?.Error,
                        Description = response.Error?.ErrorDescription
                    }
                });
            }
        }
        
        [HttpGet("GetAvailableSampleTypes")]
        public async Task<ActionResult<ResponseBase<List<LabsoftAPI.SampleType>>>> GetAvailableSampleTypes()
        {
            var response = await _samplesServices.GetAllSamples();
            
            if(response.StatusCode == 200)
            {
                var results = response.Success?.Result ?? [];

                return StatusCode(response.StatusCode, new ResponseBase<List<LabsoftAPI.SampleType>>
                {
                    Ok = response.Success != null,
                    Data = [.. results.Select(sample => sample.Sample!.SampleType)
                        .DistinctBy(m => m!.Id)
                        .Select(sampleType => new LabsoftAPI.SampleType
                        {
                            Id = sampleType!.Id,
                            Identification = sampleType.Identification
                        }).OrderBy(sampleType => sampleType.Identification)]
                });
            }
            else
            {
                LogConfiguration.CreateLogSender(HttpContext.Request, _logger, response.Error?.Exception);
                return StatusCode(response.StatusCode, new ResponseBase<dynamic>
                {
                    Ok = false,
                    Message = response.Error?.ErrorDescription,
                    Error = new ErrorBase
                    {
                        Code = response.Error?.Error,
                        Description = response.Error?.ErrorDescription
                    }
                });
            }
        }

        [HttpGet("GetAvailableBatchQCs")]
        public async Task<ActionResult<ResponseBase<List<BatchQC>>>> GetAvailableBatchQCs([FromQuery] string? numberSearch)
        {
            var response = await _samplesServices.GetAllSamples();
            
            if(response.StatusCode == 200)
            {
                var results = response.Success?.Result ?? [];

                var batchQCs = results.SelectMany(item => item.QCTests!)
                    .Select(subItem => subItem.QCTest).ToList();

                return StatusCode(response.StatusCode, new ResponseBase<List<BatchQC>>
                {
                    Ok = response.Success != null,
                    Data = batchQCs.Select(batch => batch)
                        .DistinctBy(b => b.Number)
                            .Select(batchQC => new BatchQC
                            {
                                Id = batchQC!.Number,
                                Identification = batchQC.Number.ToString()
                            }).Where(b => b.Id.ToString()!
                                .Contains(numberSearch ?? ""))
                                    .OrderBy(b => b.Id).ToList()!
                });
            }
            else
            {
                LogConfiguration.CreateLogSender(HttpContext.Request, _logger, response.Error?.Exception);
                return StatusCode(response.StatusCode, new ResponseBase<dynamic>
                {
                    Ok = false,
                    Message = response.Error?.ErrorDescription,
                    Error = new ErrorBase
                    {
                        Code = response.Error?.Error,
                        Description = response.Error?.ErrorDescription
                    }
                });
            }
        }

        [HttpGet("GetAvailableStartUsers")]
        public async Task<ActionResult<ResponseBase<List<StartUser>>>> GetAvailableStartUsers()
        {
            var response = await _samplesServices.GetAllSamples();
            
            if(response.StatusCode == 200)
            {
                var results = response.Success?.Result ?? [];

                return StatusCode(response.StatusCode, new ResponseBase<List<StartUser>>
                {
                    Ok = response.Success != null,
                    Data = results.Select(sample => sample.CurrentStatus?.StartUser)
                        .Where(user => user != null)
                        .DistinctBy(m => m?.Id)
                        .OrderBy(m => m?.Identification).ToList()!
                });
            }
            else
            {
                LogConfiguration.CreateLogSender(HttpContext.Request, _logger, response.Error?.Exception);
                return StatusCode(response.StatusCode, new ResponseBase<dynamic>
                {
                    Ok = false,
                    Message = response.Error?.ErrorDescription,
                    Error = new ErrorBase
                    {
                        Code = response.Error?.Error,
                        Description = response.Error?.ErrorDescription
                    }
                });
            }
        }

        [HttpGet("GetAvailableSampleNumbers")]
        public async Task<ActionResult<ResponseBase<List<SampleNumber>>>> GetAvailableControlNumbers([FromQuery] string? numberSearch)
        {
            var response = await _samplesServices.GetAllSamples();
        
            if (response.StatusCode == 200)
            {
                var results = response.Success?.Result ?? [];
        
                var sampleNumbers = results
                    .Where(sample => !string.IsNullOrEmpty(sample.Sample?.ControlNumber))
                    .Select(sample => new SampleNumber
                    {
                        Id = sample.Id,
                        Identification = sample.Sample?.ControlNumber
                    })
                    .DistinctBy(sample => sample.Identification)
                    .Where(sample => string.IsNullOrEmpty(numberSearch) || 
                                     (sample.Identification != null && 
                                      sample.Identification.Contains(numberSearch, StringComparison.OrdinalIgnoreCase)))
                    .OrderBy(sample => sample.Identification)
                    .ToList();
        
                return Ok(new ResponseBase<List<SampleNumber>>
                {
                    Ok = true,
                    Data = sampleNumbers
                });
            }
            else
            {
                LogConfiguration.CreateLogSender(HttpContext.Request, _logger, response.Error?.Exception);
                return StatusCode(response.StatusCode, new ResponseBase<dynamic>
                {
                    Ok = false,
                    Message = response.Error?.ErrorDescription,
                    Error = new ErrorBase
                    {
                        Code = response.Error?.Error,
                        Description = response.Error?.ErrorDescription
                    }
                });
            }
        }

        [HttpGet("GetAvailableCollectionPoints")]
        public async Task<ActionResult<ResponseBase<List<CollectionPoint>>>> GetAvailableCollectionPoints([FromQuery] string? pointSearch)
        {
            var response = await _samplesServices.GetAllSamples();

            if (response.StatusCode == 200)
            {
                var results = response.Success?.Result ?? [];

                var collectionPoints = results
                    .Select(item => item.Sample)
                    .Where(sample => sample?.CollectionPoint?.Identification != null)
                    .Select(sample => new CollectionPoint
                    {
                        Id = sample!.CollectionPoint?.Id,
                        Identification = sample.CollectionPoint?.Identification
                    })
                    .DistinctBy(sn => sn.Id)
                    .Where(sn => sn.Id.ToString()!.Contains(pointSearch ?? ""))
                    .OrderBy(sn => sn.Id)
                    .ToList();

                return StatusCode(response.StatusCode, new ResponseBase<List<CollectionPoint>>
                {
                    Ok = response.Success != null,
                    Data = collectionPoints
                });
            }
            else
            {
                LogConfiguration.CreateLogSender(HttpContext.Request, _logger, response.Error?.Exception);
                return StatusCode(response.StatusCode, new ResponseBase<dynamic>
                {
                    Ok = false,
                    Message = response.Error?.ErrorDescription,
                    Error = new ErrorBase
                    {
                        Code = response.Error?.Error,
                        Description = response.Error?.ErrorDescription
                    }
                });
            }
        }

        [HttpGet("GetAvailableSampleReasons")]
        public async Task<ActionResult<ResponseBase<List<SampleReason>>>> GetAvailableSampleReasons([FromQuery] string? reasonSearch)
        {
            // Assuming _samplesServices is a service that interacts with the data layer
            var response = await _samplesServices.GetAllSamples();

            if (response.StatusCode == 200)
            {
                var results = response.Success?.Result ?? [];

                var sampleReasons = results
                    .Select(item => item.Sample?.SampleReason)
                    .Where(reason => reason != null)
                    .DistinctBy(reason => new { reason?.Id, })
                    .Select(reason => new SampleReason
                    {
                        Id = reason?.Id,
                        Identification = reason?.Identification
                    })
                    .Where(sr => string.IsNullOrEmpty(reasonSearch) || sr.Identification!.Contains(reasonSearch, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(sr => sr.Id)
                    .ToList();

                return Ok(new ResponseBase<List<SampleReason>>
                {
                    Ok = true,
                    Data = sampleReasons
                });
            }
            else
            {
                LogConfiguration.CreateLogSender(HttpContext.Request, _logger, response.Error?.Exception);
                return StatusCode(response.StatusCode, new ResponseBase<dynamic>
                {
                    Ok = false,
                    Message = response.Error?.ErrorDescription,
                    Error = new ErrorBase
                    {
                        Code = response.Error?.Error,
                        Description = response.Error?.ErrorDescription
                    }
                });
            }
        }

        [HttpGet("GetAvailableActivities")]
        public async Task<ActionResult<ResponseBase<List<SampleActivity>>>> GetActivities([FromQuery] string? activitySearch)
        {
            // Assuming _activitiesService is a service that interacts with the data layer
            var response = await _samplesServices.GetAllSamples();
        
            if (response.StatusCode == 200)
            {
                var results = response.Success?.Result ?? [];

                // Use SelectMany to flatten the collection of SampleWorks and extract activities
                var activities = results
                    .SelectMany(item => item.Sample?.SampleWorks ?? new List<SampleWork>()) // Flatten the collection
                    .Where(activity => activity != null)
                    .DistinctBy(activity => new { activity.Work?.Id, activity.Work?.ControlNumber }) // Ensure uniqueness by Id and Name
                    .Select(activity => new Entities.SampleActivity
                    {
                        Id = activity.Work?.Id,
                        Identification = activity.Work?.ControlNumber
                    })
                    .Where(a => string.IsNullOrEmpty(activitySearch) || a.Identification!.Contains(activitySearch, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(a => a.Id)
                    .ToList();

                return Ok(new ResponseBase<List<SampleActivity>>
                {
                    Ok = true,
                    Data = activities
                });
            }
            else
            {
                LogConfiguration.CreateLogSender(HttpContext.Request, _logger, response.Error?.Exception);
                return StatusCode(response.StatusCode, new ResponseBase<dynamic>
                {
                    Ok = false,
                    Message = response.Error?.ErrorDescription,
                    Error = new ErrorBase
                    {
                        Code = response.Error?.Error,
                        Description = response.Error?.ErrorDescription
                    }
                });
            }
        }

        [HttpGet("GetForPerformTask")]
        public async Task<ActionResult<ResponseBase<TaskForPerform>>> GetForPerformTask([FromQuery] int[] sampleMethodIds) {
            var response = await _samplesServices.GetForPerformTask(sampleMethodIds);
            
            if(response.StatusCode == 200)
            {
                var results = response.Success ?? [];

                return StatusCode(response.StatusCode, new ResponseBase<List<TaskForPerform>>
                {
                    Data = results
                });
            }
            else
            {
                LogConfiguration.CreateLogSender(HttpContext.Request, _logger, response.Error?.Exception);
                return StatusCode(response.StatusCode, new ResponseBase<dynamic>
                {
                    Ok = false,
                    Message = response.Error?.ErrorDescription,
                    Error = new ErrorBase
                    {
                        Code = response.Error?.Error,
                        Description = response.Error?.ErrorDescription
                    }
                });
            }
        }

        [HttpGet("GetSamplesSummary")]
        public async Task<ActionResult<ResponseBase<SamplesSummary>>> GetSamplesSummary(
            [FromQuery] int[] sampleIds,
            [FromQuery] string[] sampleIdentifications,
            [FromQuery] int[] methodIds,
            [FromQuery] int[] stageIds,
            [FromQuery] int[] serviceAreaIds,
            [FromQuery] int[] sampleTypeIds,
            [FromQuery] int[] startUserIds,
            [FromQuery] int[] batchNumbers,
            [FromQuery] string[] customValues,
            [FromQuery] string[] sampleNumbers,
            [FromQuery] int[] collectionPoints,
            [FromQuery] int[] sampleReasons,
            [FromQuery] int[] sampleActivities,
            [FromQuery] DateTime? validityStartDate,
            [FromQuery] DateTime? validityEndDate,
            [FromQuery] DateTime? executionStartDate,
            [FromQuery] DateTime? executionEndDate,
            [FromQuery] DateTime? conclusionStartDate,
            [FromQuery] DateTime? conclusionEndDate,
            [FromQuery] DateTime? receiptStartDate,
            [FromQuery] DateTime? receiptEndDate,
            [FromQuery] DateTime? startStartDate,
            [FromQuery] DateTime? startEndDate,
            [FromQuery] DateTime? collectStartDate,
            [FromQuery] DateTime? collectEndDate,
            [FromQuery] bool useFilters = true)
        {
            var identityCenterToken = User.Claims.FirstOrDefault(c => c.Type == "nested_jwt")?.Value;
            dynamic response = useFilters ?
                await _samplesServices.GetAllSamples() :
                await _samplesServices.GetSampleMethodsCount(identityCenterToken);
            
            if(response.StatusCode == 200)
            {
                if(useFilters)
                {
                    var results = (response.Success as MyLIMSResponseBase<AnalysisSample>)?.Result ?? [];

                    if(!sampleIds.IsNullOrEmpty()) {
                        results = results.Where(item => sampleIds.ToList()
                            .Contains(item.Sample?.Id ?? 0)).ToList();
                    }

                    if(!sampleIdentifications.IsNullOrEmpty()) {
                        results = results.Where(item => sampleIdentifications.ToList()
                            .Any(identification => (item.Sample?.Identification ?? "")
                                .Contains(identification, StringComparison.OrdinalIgnoreCase)))
                                    .ToList();
                    }

                    if(!methodIds.IsNullOrEmpty()) {
                        results = results.Where(item => methodIds.ToList()
                            .Contains(item.Method?.MasterId ?? 0)).ToList();
                    }

                    if(!stageIds.IsNullOrEmpty()) {
                        results = results.Where(item => stageIds.ToList()
                            .Contains(item.CurrentStatus?.MethodStatus?.Id ?? 0)).ToList();
                    }

                    if(!serviceAreaIds.IsNullOrEmpty()) {
                        results = results.Where(item => serviceAreaIds.ToList()
                            .Contains(item.ServiceArea?.Id ?? 0)).ToList();
                    }

                    if(!sampleTypeIds.IsNullOrEmpty()) {
                        results = results.Where(item => sampleTypeIds.ToList()
                            .Contains(item?.Sample?.SampleType?.Id ?? 0)).ToList();
                    }

                    if(!startUserIds.IsNullOrEmpty()) {
                        results = results.Where(item => startUserIds.ToList()
                            .Contains(item?.CurrentStatus?.StartUser?.Id ?? 0)).ToList();
                    }

                    if(!batchNumbers.IsNullOrEmpty()) {
                        results = results.Where(item => (item.QCTests ?? [])
                            .Any(q => batchNumbers.Contains(q.QCTest.Number ?? 0))).ToList();
                    }

                    if(!customValues.IsNullOrEmpty()) {
                        results = results.Where(item => customValues.ToList()
                            .Any(value => (item.SampleCustomInfo?.DisplayValue ?? "")
                                .Contains(value, StringComparison.OrdinalIgnoreCase)))
                                    .ToList();
                    }
                                    
                    if(!sampleNumbers.IsNullOrEmpty()) {
                        results = results.Where(item => sampleNumbers.ToList()
                                .Contains(item?.Sample?.ControlNumber ?? "")).ToList();
                    }

                    if(!collectionPoints.IsNullOrEmpty()) {
                        results = results.Where(item => collectionPoints.ToList()
                                .Contains(item?.Sample?.CollectionPoint?.Id ?? 0)).ToList();
                    }

                    if(!sampleReasons.IsNullOrEmpty()) {
                        results = results.Where(item => sampleReasons.ToList()
                                .Contains(item?.Sample?.SampleReason?.Id ?? 0)).ToList();
                    }

                    if(!sampleActivities.IsNullOrEmpty()) {
                        results = results.Where(item => (item.Sample?.SampleWorks ?? [])
                                .Any(q => sampleActivities.Contains(q.Work?.Id ?? 0))).ToList();
                    }

                    if(validityStartDate != null) {
                        results = results.Where(
                            item => item.AnalysisDeadline >= validityStartDate &&
                            item.AnalysisDeadline <= validityEndDate).ToList();
                    }

                    if(executionStartDate != null) {
                        results = results.Where(
                            item => item.CurrentStatus?.ExecuteDateTime >= executionStartDate &&
                            item.CurrentStatus.ExecuteDateTime <= executionEndDate).ToList();
                    }

                    if(conclusionStartDate != null) {
                        results = results.Where(
                            item => item.Conclusion >= conclusionStartDate &&
                            item.Conclusion <= conclusionEndDate).ToList();
                    }

                    if(receiptStartDate != null) {
                        results = results.Where(
                            item => item.Sample?.ReceivedTime >= receiptStartDate &&
                            item.Sample.ReceivedTime <= receiptEndDate).ToList();
                    }

                    if(startStartDate != null) {
                        results = results.Where(
                            item => item.CurrentStatus?.StartDateTime >= startStartDate &&
                            item.CurrentStatus.StartDateTime <= startEndDate).ToList();
                    }

                    if(collectStartDate != null) {
                        results = results.Where(
                            item => item.Sample?.TakenDateTime >= collectStartDate &&
                            item.Sample.TakenDateTime <= collectEndDate).ToList();
                    }

                    return StatusCode(response.StatusCode, new ResponseBase<SamplesSummary>
                    {
                        Ok = response.Success != null,
                        Data = new SamplesSummary
                        {
                            PrepareAnalysis = results.Where(item =>
                                item?.CurrentStatus?.MethodStatus?.MethodStatusBehaviorId == (int) SampleType.prepareAnalysis)
                                .Select(item => item.Sample!).Count(),
                            CarriedOutAnalysis = results.Where(item =>
                                item?.CurrentStatus?.MethodStatus?.MethodStatusBehaviorId == (int) SampleType.carriedOutAnalysis)
                                .Select(item => item.Sample!).Count(),
                            ReviewAnalysis = results.Where(item =>
                                item?.CurrentStatus?.MethodStatus?.MethodStatusBehaviorId == (int) SampleType.reviewAnalysis)
                                .Select(item => item.Sample!).Count(),
                            BatchQC = results.SelectMany(item => item.QCTests!)
                                .Select(subItem => subItem.QCTest).Count()
                        }
                    });
                }
                else
                {
                    var results = response.Success as SampleMethodsCount;
                    return StatusCode(response.StatusCode, new ResponseBase<SamplesSummary>
                    {
                        Ok = response.Success != null,
                        Data = new SamplesSummary
                        {
                            PrepareAnalysis = results?.SampleMethodsToPrepareCount ?? 0,
                            CarriedOutAnalysis = results?.SampleMethodsToExecuteCount ?? 0,
                            ReviewAnalysis = results?.SampleMethodsToReviewCount ?? 0,
                            BatchQC = results?.AvaliableQcTestsCount ?? 0
                        }
                    });
                }
            }
            else
            {
                LogConfiguration.CreateLogSender(HttpContext.Request, _logger, response.Error?.Exception);
                return StatusCode(response.StatusCode, new ResponseBase<dynamic>
                {
                    Ok = false,
                    Message = response.Error?.ErrorDescription,
                    Error = new ErrorBase
                    {
                        Code = response.Error?.Error,
                        Description = response.Error?.ErrorDescription
                    }
                });
            }
        }

        [HttpGet("GetSamples")]
        public async Task<ActionResult<ResponseBase<Pagination<SampleDTO>>>> GetSamples(
            [FromQuery] string? sampleType,
            [FromQuery] string? sortParam,
            [FromQuery] int[] sampleIds,
            [FromQuery] string[] sampleIdentifications,
            [FromQuery] int[] methodIds,
            [FromQuery] int[] stageIds,
            [FromQuery] int[] serviceAreaIds,
            [FromQuery] int[] sampleTypeIds,
            [FromQuery] int[] startUserIds,
            [FromQuery] int[] batchNumbers,
            [FromQuery] string[] customValues,
            [FromQuery] string[] sampleNumbers,
            [FromQuery] int[] collectionPoints,
            [FromQuery] int[] sampleReasons,
            [FromQuery] int[] sampleActivities,
            [FromQuery] DateTime? priorityStartDate,
            [FromQuery] DateTime? priorityEndDate,
            [FromQuery] DateTime? validityStartDate,
            [FromQuery] DateTime? validityEndDate,
            [FromQuery] DateTime? executionStartDate,
            [FromQuery] DateTime? executionEndDate,
            [FromQuery] DateTime? conclusionStartDate,
            [FromQuery] DateTime? conclusionEndDate,
            [FromQuery] DateTime? receiptStartDate,
            [FromQuery] DateTime? receiptEndDate,
            [FromQuery] DateTime? startStartDate,
            [FromQuery] DateTime? startEndDate,
            [FromQuery] DateTime? collectStartDate,
            [FromQuery] DateTime? collectEndDate,
            [FromQuery] int perPage = 10,
            [FromQuery] int page = 1)
        {
            if (Enum.TryParse<SampleType>(sampleType, out var type))
            {
                if (Enum.TryParse<SampleSortParam>(sortParam, out var param)) {
                    switch(param) {
                        case SampleSortParam.batchQC:
                            sortParam = "QCTest/Id";
                            break;
                        case SampleSortParam.id:
                            sortParam = "Sample/Id";
                            break;
                        case SampleSortParam.analyticsMethod:
                            sortParam = "Method/MasterId";
                            break;
                        case SampleSortParam.sampleType:
                            sortParam = "Sample/SampleType/Id";
                            break;
                        case SampleSortParam.stage:
                            sortParam = "CurrentStatus/MethodStatus/Id";
                            break;
                        case SampleSortParam.serviceArea:
                            sortParam = "ServiceArea/Id";
                            break;
                        case SampleSortParam.startUser:
                            sortParam = "CurrentStatus/StartUser/Id";
                            break;
                        case SampleSortParam.date:
                            sortParam = "Sample/ReceivedTime";
                            break;
                        case SampleSortParam.sampleNumber:
                            sortParam = "Sample/ControlNumber";
                            break;
                        case SampleSortParam.sampleIdentification:
                            sortParam = "Sample/Identification";
                            break;
                        case SampleSortParam.customInfo:
                            sortParam = "SampleCustomInfo/DisplayValue";
                            break;
                        case SampleSortParam.activities:
                            sortParam = "Sample/SampleWorks";
                            break;
                        case SampleSortParam.collectionPoint:
                            sortParam = "Sample/CollectionPoint/Id";
                            break;
                        case SampleSortParam.sampleReason:
                            sortParam = "Sample/SampleReason/Id";
                            break;
                    }
                }

                string filter = "";

                if(!sampleIds.IsNullOrEmpty()) {
                    List<string> values = [];
                    foreach(int id in sampleIds) {
                        values.Add($"Sample/Id eq {id}");
                    }

                    filter += $"({string.Join(" or ", values)}) and ";
                }

                if(!sampleIdentifications.IsNullOrEmpty()) {
                    List<string> values = [];
                    foreach(string identification in sampleIdentifications) {
                        values.Add($"substringof('{identification}', Sample/Identification)");
                    }

                    filter += $"({string.Join(" or ", values)}) and ";
                }

                if(!methodIds.IsNullOrEmpty()) {
                    List<string> values = [];
                    foreach(int id in methodIds) {
                        values.Add($"Method/MasterId eq {id}");
                    }

                    filter += $"({string.Join(" or ", values)}) and ";
                }

                if(!stageIds.IsNullOrEmpty()) {
                    List<string> values = [];
                    foreach(int id in stageIds) {
                        values.Add($"CurrentStatus/MethodStatus/Id eq {id}");
                    }

                    filter += $"({string.Join(" or ", values)}) and ";
                }

                if(!serviceAreaIds.IsNullOrEmpty()) {
                    List<string> values = [];
                    foreach(int id in serviceAreaIds) {
                        values.Add($"ServiceArea/Id eq {id}");
                    }

                    filter += $"({string.Join(" or ", values)}) and ";
                }

                if(!sampleTypeIds.IsNullOrEmpty()) {
                    List<string> values = [];
                    foreach(int id in sampleTypeIds) {
                        values.Add($"Sample/SampleType/Id eq {id}");
                    }

                    filter += $"({string.Join(" or ", values)}) and ";
                }

                if(!startUserIds.IsNullOrEmpty()) {
                    List<string> values = [];
                    foreach(int id in startUserIds) {
                        values.Add($"CurrentStatus/StartUser/Id eq {id}");
                    }

                    filter += $"({string.Join(" or ", values)}) and ";
                }

                if(!batchNumbers.IsNullOrEmpty()) {
                    List<string> values = [];
                    foreach(int number in batchNumbers) {
                        values.Add($"substringof('{number}', b/QCTest/Number)");
                    }

                    filter += $"QCTests/any(b: {string.Join(" or ", values)}) and ";
                }

                if(!customValues.IsNullOrEmpty()) {
                    List<string> values = [];
                    foreach(string value in customValues) {
                        values.Add($"SampleCustomInfo/DisplayValue eq {value}");
                    }

                    filter += $"({string.Join(" or ", values)}) and ";
                }

                if(!sampleNumbers.IsNullOrEmpty()) {
                    List<string> values = [];
                    foreach(string number in sampleNumbers) {
                        values.Add($"substringof('{number}', Sample/ControlNumber)");
                    }

                    filter += $"({string.Join(" or ", values)}) and ";
                }

                if(!collectionPoints.IsNullOrEmpty()) {
                    List<string> values = [];
                    foreach(int id in collectionPoints) {
                        values.Add($"Sample/CollectionPoint/Id eq {id}");
                    }

                    filter += $"({string.Join(" or ", values)}) and ";
                }

                if(!sampleReasons.IsNullOrEmpty()) {
                    List<string> values = [];
                    foreach(int id in sampleReasons) {
                        values.Add($"Sample/SampleReason/Id eq {id}");
                    }

                    filter += $"({string.Join(" or ", values)}) and ";
                }

                if(!sampleActivities.IsNullOrEmpty()) {
                    List<string> values = [];
                    foreach(int id in sampleActivities) {
                        values.Add($"b/Work/Id eq {id}");
                    }

                    filter += $"Sample/SampleWorks/any(b: {string.Join(" or ", values)}) and ";
                }

                if(priorityStartDate != null) {
                    filter += $"(PriorityDate gt datetime'{priorityStartDate?.ToString("yyyy-MM-ddTHH:mm:ss")}' ";
                    filter += $"and PriorityDate lt datetime'{priorityEndDate?.ToString("yyyy-MM-ddTHH:mm:ss")}') and ";
                }

                if(validityStartDate != null) {
                    filter += $"(AnalysisDeadline gt datetime'{validityStartDate?.ToString("yyyy-MM-ddTHH:mm:ss")}' ";
                    filter += $"and AnalysisDeadline lt datetime'{validityEndDate?.ToString("yyyy-MM-ddTHH:mm:ss")}') and ";
                }

                if(executionStartDate != null) {
                    filter += $"(CurrentStatus/ExecuteDateTime gt datetime'{executionStartDate?.ToString("yyyy-MM-ddTHH:mm:ss")}' ";
                    filter += $"and CurrentStatus/ExecuteDateTime lt datetime'{executionEndDate?.ToString("yyyy-MM-ddTHH:mm:ss")}') and ";
                }

                if(conclusionStartDate != null) {
                    filter += $"(Conclusion gt datetime'{conclusionStartDate?.ToString("yyyy-MM-ddTHH:mm:ss")}' ";
                    filter += $"and Conclusion lt datetime'{conclusionEndDate?.ToString("yyyy-MM-ddTHH:mm:ss")}') and ";
                }

                if(receiptStartDate != null) {
                    filter += $"(Sample/ReceivedTime gt datetime'{receiptStartDate?.ToString("yyyy-MM-ddTHH:mm:ss")}' ";
                    filter += $"and Sample/ReceivedTime lt datetime'{receiptEndDate?.ToString("yyyy-MM-ddTHH:mm:ss")}') and ";
                }

                if(startStartDate != null) {
                    filter += $"(CurrentStatus/StartDateTime gt datetime'{startStartDate?.ToString("yyyy-MM-ddTHH:mm:ss")}' ";
                    filter += $"and CurrentStatus/StartDateTime lt datetime'{startEndDate?.ToString("yyyy-MM-ddTHH:mm:ss")}') and ";
                }

                if(collectStartDate != null) {
                    filter += $"(Sample/TakenDateTime gt datetime'{collectStartDate?.ToString("yyyy-MM-ddTHH:mm:ss")}' ";
                    filter += $"and Sample/TakenDateTime lt datetime'{collectEndDate?.ToString("yyyy-MM-ddTHH:mm:ss")}') and ";
                }

                if(filter.Length > 0) {
                    filter = filter[..^5];
                }
                var identityCenterToken = User.Claims.FirstOrDefault(c => c.Type == "nested_jwt")?.Value;
                ExternalResponse<MyLIMSResponseBase<AnalysisSample>, ErrorResponse> response;
                response = await _samplesServices.GetForExecutionSamples(
                    identityCenterToken, (int) type, sortParam, filter,  perPage, (page - 1) * perPage);

                if(response.StatusCode == 200)
                {
                    var results = response.Success?.Result ?? [];
                    
                    var samples = results.Select(item => new SampleDTO
                    {
                        Id = item.Sample?.Id,
                        SampleMethodId = item.Id,
                        Identification = item.Sample?.Identification,
                        Conclusion = item.Sample?.Conclusion,
                        TakenDateTime = item.Sample?.TakenDateTime,
                        ReceivedTime = item.Sample?.ReceivedTime,
                        QCTests = item.QCTests,
                        CurrentStatus = new CurrentStatus
                        {
                            Id = item.Sample?.CurrentStatus?.Id,
                            SampleStatus = new SampleStatus
                            {
                                Id = item.Sample?.CurrentStatus?.SampleStatus?.Id,
                                Identification = item.Sample?.CurrentStatus?.SampleStatus?.Identification,
                                BeforeReceive = item.Sample?.CurrentStatus?.SampleStatus?.BeforeReceive,
                                AfterPublish = item.Sample?.CurrentStatus?.SampleStatus?.AfterPublish,
                                PortalSampleStatus = item.Sample?.CurrentStatus?.SampleStatus?.PortalSampleStatus
                            }
                        },
                        ServiceArea = new SampleServiceArea
                        {
                            ExtraTime = item.ServiceArea?.ExtraTime,
                            ExternalServiceArea = item.ServiceArea?.ExternalServiceArea,
                            Active = item.ServiceArea?.Active,
                            Id = item.ServiceArea?.Id,
                            Identification = item.ServiceArea?.Identification
                        },
                        SampleType = new LabsoftAPI.SampleType
                        {
                            Id = item.Sample?.SampleType?.Id,
                            Identification = item.Sample?.SampleType?.Identification
                        },
                        Method = new Method
                        {
                            MasterId = item.Method?.MasterId,
                            Id = item.Method?.Id,
                            Identification = item.Method?.Identification,
                            QCRoutineBatchIds = item.Method?.QCRoutineBatchIds
                        }
                    }).ToList();

                    return StatusCode(response.StatusCode, new ResponseBase<Pagination<SampleDTO>>
                    {
                        Data = new Pagination<SampleDTO>
                        {
                            CurrentPage = page,
                            PerPage = perPage,
                            TotalPages = (int) Math.Ceiling((double) (response.Success?.TotalCount ?? 0) / perPage),
                            TotalItems = response.Success?.TotalCount ?? 0,
                            Items = samples
                        }
                    });
                }
                else
                {
                    LogConfiguration.CreateLogSender(HttpContext.Request, _logger, response.Error?.Exception);
                    return StatusCode(response.StatusCode, new ResponseBase<dynamic>
                    {
                        Ok = false,
                        Message = response.Error?.ErrorDescription,
                        Error = new ErrorBase
                        {
                            Code = response.Error?.Error,
                            Description = response.Error?.ErrorDescription
                        }
                    });
                }
            }
            else
            {
                return BadRequest(new ResponseBase<dynamic>
                {
                    Ok = false,
                    Message = "invalid_sample_type",
                    Error = new ErrorBase
                    {
                        Code = "bad_request",
                        Description = "invalid_sample_type"
                    }
                });
            }
        }

        [HttpGet("GetBatchQCs")]
        public async Task<ActionResult<ResponseBase<Pagination<QCTest>>>> GetBatchQCs(
            [FromQuery] int[] sampleIds,
            [FromQuery] string[] sampleIdentifications,
            [FromQuery] int[] methodIds,
            [FromQuery] int[] stageIds,
            [FromQuery] int[] serviceAreaIds,
            [FromQuery] int[] sampleTypeIds,
            [FromQuery] int[] startUserIds,
            [FromQuery] int[] batchNumbers,
            [FromQuery] string[] customValues,
            [FromQuery] string[] sampleNumbers,
            [FromQuery] int[] collectionPoints,
            [FromQuery] int[] sampleReasons,
            [FromQuery] int[] sampleActivities,
            [FromQuery] DateTime? validityStartDate,
            [FromQuery] DateTime? validityEndDate,
            [FromQuery] DateTime? executionStartDate,
            [FromQuery] DateTime? executionEndDate,
            [FromQuery] DateTime? conclusionStartDate,
            [FromQuery] DateTime? conclusionEndDate,
            [FromQuery] DateTime? receiptStartDate,
            [FromQuery] DateTime? receiptEndDate,
            [FromQuery] DateTime? startStartDate,
            [FromQuery] DateTime? startEndDate,
            [FromQuery] DateTime? collectStartDate,
            [FromQuery] DateTime? collectEndDate,
            [FromQuery] int perPage = 10,
            [FromQuery] int page = 1)
        {
            var response = await _samplesServices.GetAllSamples();
            
            if(response.StatusCode == 200)
            {
                var results = response.Success?.Result ?? [];

                if(!sampleIds.IsNullOrEmpty()) {
                    results = results.Where(item => sampleIds.ToList()
                        .Contains(item.Sample?.Id ?? 0)).ToList();
                }

                if(!sampleIdentifications.IsNullOrEmpty()) {
                    results = results.Where(item => sampleIdentifications.ToList()
                        .Any(identification => (item.Sample?.Identification ?? "")
                            .Contains(identification, StringComparison.OrdinalIgnoreCase)))
                                .ToList();
                }

                if(!methodIds.IsNullOrEmpty()) {
                    results = results.Where(item => methodIds.ToList()
                        .Contains(item.Method?.MasterId ?? 0)).ToList();
                }

                if(!stageIds.IsNullOrEmpty()) {
                    results = results.Where(item => stageIds.ToList()
                        .Contains(item.CurrentStatus?.MethodStatus?.Id ?? 0)).ToList();
                }

                if(!serviceAreaIds.IsNullOrEmpty()) {
                    results = results.Where(item => serviceAreaIds.ToList()
                        .Contains(item.ServiceArea?.Id ?? 0)).ToList();
                }

                if(!sampleTypeIds.IsNullOrEmpty()) {
                    results = results.Where(item => sampleTypeIds.ToList()
                        .Contains(item?.Sample?.SampleType?.Id ?? 0)).ToList();
                }

                if(!startUserIds.IsNullOrEmpty()) {
                    results = results.Where(item => startUserIds.ToList()
                        .Contains(item?.CurrentStatus?.StartUser?.Id ?? 0)).ToList();
                }
                
                if(!sampleNumbers.IsNullOrEmpty()) {
                        results = results.Where(item => sampleNumbers.ToList()
                            .Contains(item?.Sample?.ControlNumber ?? "")).ToList();
                }

                if(!batchNumbers.IsNullOrEmpty()) {
                    results = results.Where(item => (item.QCTests ?? [])
                        .Any(q => batchNumbers.Contains(q.QCTest.Number ?? 0))).ToList();
                }

                if(!customValues.IsNullOrEmpty()) {
                    results = results.Where(item => customValues.ToList()
                        .Any(value => (item.SampleCustomInfo?.DisplayValue ?? "")
                            .Contains(value, StringComparison.OrdinalIgnoreCase)))
                                .ToList();
                }
                
                if(!collectionPoints.IsNullOrEmpty()) {
                    results = results.Where(item => collectionPoints.ToList()
                            .Contains(item?.Sample?.CollectionPoint?.Id ?? 0)).ToList();
                }

                if(!sampleReasons.IsNullOrEmpty()) {
                    results = results.Where(item => sampleReasons.ToList()
                            .Contains(item?.Sample?.SampleReason?.Id ?? 0)).ToList();
                }

                if(!sampleActivities.IsNullOrEmpty()) {
                    results = results.Where(item => (item.Sample?.SampleWorks ?? [])
                            .Any(q => sampleActivities.Contains(q.Work?.Id ?? 0))).ToList();
                }

                if(validityStartDate != null) {
                    results = results.Where(
                        item => item.AnalysisDeadline >= validityStartDate &&
                        item.AnalysisDeadline <= validityEndDate).ToList();
                }

                if(executionStartDate != null) {
                    results = results.Where(
                        item => item.CurrentStatus?.ExecuteDateTime >= executionStartDate &&
                        item.CurrentStatus.ExecuteDateTime <= executionEndDate).ToList();
                }

                if(conclusionStartDate != null) {
                    results = results.Where(
                        item => item.Conclusion >= conclusionStartDate &&
                        item.Conclusion <= conclusionEndDate).ToList();
                }

                if(receiptStartDate != null) {
                    results = results.Where(
                        item => item.Sample?.ReceivedTime >= receiptStartDate &&
                        item.Sample.ReceivedTime <= receiptEndDate).ToList();
                }

                if(startStartDate != null) {
                    results = results.Where(
                        item => item.CurrentStatus?.StartDateTime >= startStartDate &&
                        item.CurrentStatus.StartDateTime <= startEndDate).ToList();
                }

                if(collectStartDate != null) {
                    results = results.Where(
                        item => item.Sample?.TakenDateTime >= collectStartDate &&
                        item.Sample.TakenDateTime <= collectEndDate).ToList();
                }

                var batchQCs = results.SelectMany(item => item.QCTests!)
                    .Select(subItem => subItem.QCTest).ToList();

                return StatusCode(response.StatusCode, new ResponseBase<Pagination<QCTest>>
                {
                    Data = new Pagination<QCTest>
                    {
                        CurrentPage = page,
                        PerPage = perPage,
                        TotalPages = (int) Math.Ceiling((double) batchQCs.Count / perPage),
                        TotalItems = batchQCs.Count,
                        Items = batchQCs.Skip((page - 1) * perPage)
                            .Take(perPage).ToList()
                    }
                });
            }
            else
            {
                LogConfiguration.CreateLogSender(HttpContext.Request, _logger, response.Error?.Exception);
                return StatusCode(response.StatusCode, new ResponseBase<dynamic>
                {
                    Ok = false,
                    Message = response.Error?.ErrorDescription,
                    Error = new ErrorBase
                    {
                        Code = response.Error?.Error,
                        Description = response.Error?.ErrorDescription
                    }
                });
            }
        }

        [HttpGet("ValidateSampleCode")]
        public async Task<ActionResult<ResponseBase<SampleDTO>>> ValidateSampleCode(int code)
        {
            var response = await _samplesServices.ValidateSampleCode(code);
            
            // Validate success data because if code not match with any sample the API response with status 200
            if(response.Success != null)
            {
                return StatusCode(response.StatusCode, new ResponseBase<SampleDTO>
                {
                    Ok = response.Success != null,
                    Data = new SampleDTO
                    {
                        Id = response.Success?.Sample?.Id,
                        Identification = response.Success?.Sample?.Identification,
                        Conclusion = response.Success?.Sample?.Conclusion,
                        TakenDateTime = response.Success?.Sample?.TakenDateTime,
                        ReceivedTime = response.Success?.Sample?.ReceivedTime,
                        CurrentStatus = new CurrentStatus
                        {
                            Id = response.Success?.Sample?.CurrentStatus?.Id,
                            SampleStatus = new SampleStatus
                            {
                                Id = response.Success?.Sample?.CurrentStatus?.SampleStatus?.Id,
                                Identification = response.Success?.Sample?.CurrentStatus?.SampleStatus?.Identification,
                                BeforeReceive = response.Success?.Sample?.CurrentStatus?.SampleStatus?.BeforeReceive,
                                AfterPublish = response.Success?.Sample?.CurrentStatus?.SampleStatus?.AfterPublish,
                                PortalSampleStatus = response.Success?.Sample?.CurrentStatus?.SampleStatus?.PortalSampleStatus
                            }
                        },
                        ServiceArea = new SampleServiceArea
                        {
                            ExtraTime = response.Success?.ServiceArea?.ExtraTime,
                            ExternalServiceArea = response.Success?.ServiceArea?.ExternalServiceArea,
                            Active = response.Success?.ServiceArea?.Active,
                            Id = response.Success?.ServiceArea?.Id,
                            Identification = response.Success?.ServiceArea?.Identification
                        },
                        SampleType = new LabsoftAPI.SampleType
                        {
                            Id = response.Success?.Sample?.SampleType?.Id,
                            Identification = response.Success?.Sample?.SampleType?.Identification
                        },
                        Method = new Method
                        {
                            MasterId = response.Success?.Method?.MasterId,
                            Id = response.Success?.Method?.Id,
                            Identification = response.Success?.Method?.Identification
                        }
                    }
                });
            }
            else
            {
                LogConfiguration.CreateLogSender(HttpContext.Request, _logger, response.Error?.Exception);
                return StatusCode(response.StatusCode, new ResponseBase<dynamic>
                {
                    Ok = false,
                    Message = "sample_not_exists",
                    Error = new ErrorBase
                    {
                        Code = "not_exists",
                        Description = "sample_not_exists"
                    }
                });
            }
        }

        [HttpGet("GetAvailableQCTestsByRoutineBatchId")]
        public async Task<ActionResult<ResponseBase<List<QCTest>>>> GetAvailableQCTestsByRoutineBatchId(int routineBatchId)
        {
            var response = await _samplesServices.GetAvailableQCTestsByRoutineBatchId(routineBatchId);
            
            if(response.StatusCode == 200)
            {
                var results = response.Success ?? [];

                return StatusCode(response.StatusCode, new ResponseBase<List<QCTest>>
                {
                    Ok = response.Success != null,
                    Data = results
                });
            }
            else
            {
                LogConfiguration.CreateLogSender(HttpContext.Request, _logger, response.Error?.Exception);
                return StatusCode(response.StatusCode, new ResponseBase<dynamic>
                {
                    Ok = false,
                    Message = response.Error?.ErrorDescription,
                    Error = new ErrorBase
                    {
                        Code = response.Error?.Error,
                        Description = response.Error?.ErrorDescription
                    }
                });
            }
        }

        [HttpPost("PerformTask")]
        public async Task<ActionResult<ResponseBase<string>>> PerformTask([FromQuery] bool calculate, [FromBody] PerformTaskDTO body)
        {
            var response = await _samplesServices.PerformTask(calculate, body);
            
            if(response.StatusCode == 200)
            {
                var results = response.Success;

                return StatusCode(response.StatusCode, new ResponseBase<string>
                {
                    Data = results
                });
            }
            else
            {
                LogConfiguration.CreateLogSender(HttpContext.Request, _logger, response.Error?.Exception);
                return StatusCode(response.StatusCode, new ResponseBase<dynamic>
                {
                    Ok = false,
                    Message = response.Error?.ErrorDescription,
                    Error = new ErrorBase
                    {
                        Code = response.Error?.Error,
                        Description = response.Error?.ErrorDescription
                    }
                });
            }
        }

        [HttpPost("Method/{sampleMethodId}/AdvanceStatus")]
        public async Task<ActionResult<ResponseBase<string>>> AdvanceStatus(int sampleMethodId, [FromBody] AdvanceMethodStatusParamsDTO body)
        {
            var response = await _samplesServices.AdvanceStatus(sampleMethodId, body);
            
            if(response.StatusCode == 200)
            {
                var results = response.Success;

                return StatusCode(response.StatusCode, new ResponseBase<string>
                {
                    Data = results
                });
            }
            else
            {
                LogConfiguration.CreateLogSender(HttpContext.Request, _logger, response.Error?.Exception);
                return StatusCode(response.StatusCode, new ResponseBase<dynamic>
                {
                    Ok = false,
                    Message = response.Error?.ErrorDescription,
                    Error = new ErrorBase
                    {
                        Code = response.Error?.Error,
                        Description = response.Error?.ErrorDescription
                    }
                });
            }
        }
    }
}