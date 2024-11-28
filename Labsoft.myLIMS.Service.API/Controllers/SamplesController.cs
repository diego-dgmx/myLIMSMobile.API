using Entities;
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
    public class SamplesController(ISamplesServices samplesServices) : ControllerBase
    {
        private readonly ISamplesServices _samplesServices = samplesServices;
        
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
                var results = response.Success ?? [];

                return StatusCode(response.StatusCode, new ResponseBase<List<MethodStatus>>
                {
                    Ok = response.Success != null,
                    Data = results.Select(sample => sample.CurrentStatus!.MethodStatus)
                        .DistinctBy(m => m!.Id).OrderBy(m => m?.Identification).ToList()!
                });
            }
            else
            {
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
                var results = response.Success ?? [];

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
                var results = response.Success ?? [];

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
                var results = response.Success ?? [];

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
                var results = response.Success ?? [];

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
                var results = response.Success ?? [];
        
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
                var results = response.Success ?? [];

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
                var results = response.Success ?? [];

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
                var results = response.Success ?? [];

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
            [FromQuery] DateTime? collectEndDate)
        {
            var response = await _samplesServices.GetAllSamples();
            
            if(response.StatusCode == 200)
            {
                var results = response.Success ?? [];

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
                var response = await _samplesServices.GetAllSamples((int) type);

                if(response.StatusCode == 200)
                {
                    var results = response.Success ?? [];

                    if (Enum.TryParse<SampleSortParam>(sortParam, out var param)) {
                        switch(param) {
                            case SampleSortParam.batchQC:
                                results = [.. results.OrderBy(r => r.Sample?.Id)];
                                break;
                            case SampleSortParam.id:
                                results = [.. results.OrderBy(r => r.Sample?.Id)];
                                break;
                            case SampleSortParam.analyticsMethod:
                                results = [.. results.OrderBy(r => r.Method?.MasterId)];
                                break;
                            case SampleSortParam.sampleType:
                                results = [.. results.OrderBy(r => r.Sample?.SampleType?.Id)];
                                break;
                            case SampleSortParam.stage:
                                results = [.. results.OrderBy(r => r.CurrentStatus?.MethodStatus?.Id)];
                                break;
                            case SampleSortParam.serviceArea:
                                results = [.. results.OrderBy(r => r.ServiceArea?.Id)];
                                break;
                            case SampleSortParam.startUser:
                                results = [.. results.OrderBy(r => r.CurrentStatus?.StartUser?.Id)];
                                break;
                            case SampleSortParam.date:
                                results = [.. results.OrderBy(r => r.AnalysisDeadline)];
                                break;
                            case SampleSortParam.sampleNumber:
                                results = [.. results.OrderBy(r => r.Sample?.ControlNumber)];
                                break;
                            case SampleSortParam.sampleIdentification:
                                results = [.. results.OrderBy(r => r.Sample?.Identification)];
                                break;
                            case SampleSortParam.customInfo:
                                results = [.. results.OrderBy(r => r.SampleCustomInfo?.DisplayValue)];
                                break;
                            case SampleSortParam.activities:
                                results = [.. results.OrderBy(r => r.Sample?.Id)];
                                break;
                            case SampleSortParam.collectionPoint:
                                results = [.. results.OrderBy(r => r.Sample?.CollectionPoint?.Id)];
                                break;
                            case SampleSortParam.sampleReason:
                                results = [.. results.OrderBy(r => r.Sample?.SampleReason?.Id)];
                                break;
                        }
                    }

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
                            TotalPages = (int) Math.Ceiling((double) samples.Count / perPage),
                            TotalItems = samples.Count,
                            Items = samples.Skip((page - 1) * perPage)
                                .Take(perPage).ToList()
                        }
                    });
                    
                }
                else
                {
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
                var results = response.Success ?? [];

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