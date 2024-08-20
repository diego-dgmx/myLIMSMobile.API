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

        [HttpGet("GetAvailableSampleTypes")]
        public async Task<ActionResult<ResponseBase<List<Entities.SampleType>>>> GetAvailableSampleTypes()
        {
            var response = await _samplesServices.GetAllSamples();
            
            if(response.StatusCode == 200)
            {
                var results = response.Success ?? [];

                return StatusCode(response.StatusCode, new ResponseBase<List<Entities.SampleType>>
                {
                    Ok = response.Success != null,
                    Data = [.. results.Select(sample => sample.Sample!.SampleType)
                        .DistinctBy(m => m!.Id)
                        .Select(sampleType => new Entities.SampleType
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

        [HttpGet("GetSamplesSummary")]
        public async Task<ActionResult<ResponseBase<SamplesSummary>>> GetSamplesSummary(
            [FromQuery] int[] sampleIds,
            [FromQuery] int[] methodIds,
            [FromQuery] int[] stageIds,
            [FromQuery] int[] sampleTypeIds,
            [FromQuery] int[] batchNumbers,
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

                if(!methodIds.IsNullOrEmpty()) {
                    results = results.Where(item => methodIds.ToList()
                        .Contains(item.Method?.MasterId ?? 0)).ToList();
                }

                if(!stageIds.IsNullOrEmpty()) {
                    results = results.Where(item => stageIds.ToList()
                        .Contains(item.CurrentStatus?.MethodStatus?.Id ?? 0)).ToList();
                }

                if(!sampleTypeIds.IsNullOrEmpty()) {
                    results = results.Where(item => sampleTypeIds.ToList()
                        .Contains(item?.Sample?.SampleType?.Id ?? 0)).ToList();
                }

                if(!batchNumbers.IsNullOrEmpty()) {
                    results = results.Where(item => (item.QCTests ?? [])
                        .Any(q => batchNumbers.Contains(q.QCTest.Number ?? 0))).ToList();
                }

                if(validityStartDate != null && validityStartDate != null) {
                    results = results.Where(
                        item => item.AnalysisDeadline >= validityStartDate &&
                        item.AnalysisDeadline <= validityEndDate).ToList();
                }

                if(executionStartDate != null && executionStartDate != null) {
                    results = results.Where(
                        item => item.CurrentStatus?.ExecuteDateTime >= executionStartDate &&
                        item.CurrentStatus.ExecuteDateTime <= executionEndDate).ToList();
                }

                if(conclusionStartDate != null && conclusionStartDate != null) {
                    results = results.Where(
                        item => item.Conclusion >= conclusionStartDate &&
                        item.Conclusion <= conclusionEndDate).ToList();
                }

                if(receiptStartDate != null && receiptStartDate != null) {
                    results = results.Where(
                        item => item.Sample?.ReceivedTime >= receiptStartDate &&
                        item.Sample.ReceivedTime <= receiptEndDate).ToList();
                }

                if(startStartDate != null && startStartDate != null) {
                    results = results.Where(
                        item => item.CurrentStatus?.StartDateTime >= startStartDate &&
                        item.CurrentStatus.StartDateTime <= startEndDate).ToList();
                }

                if(collectStartDate != null && collectStartDate != null) {
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
            [FromQuery] int[] sampleIds,
            [FromQuery] int[] methodIds,
            [FromQuery] int[] stageIds,
            [FromQuery] int[] sampleTypeIds,
            [FromQuery] int[] batchNumbers,
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

                    if(!sampleIds.IsNullOrEmpty()) {
                        results = results.Where(item => sampleIds.ToList()
                            .Contains(item.Sample?.Id ?? 0)).ToList();
                    }

                    if(!methodIds.IsNullOrEmpty()) {
                        results = results.Where(item => methodIds.ToList()
                            .Contains(item.Method?.MasterId ?? 0)).ToList();
                    }

                    if(!stageIds.IsNullOrEmpty()) {
                        results = results.Where(item => stageIds.ToList()
                            .Contains(item.CurrentStatus?.MethodStatus?.Id ?? 0)).ToList();
                    }

                    if(!sampleTypeIds.IsNullOrEmpty()) {
                        results = results.Where(item => sampleTypeIds.ToList()
                            .Contains(item?.Sample?.SampleType?.Id ?? 0)).ToList();
                    }

                    if(!batchNumbers.IsNullOrEmpty()) {
                        results = results.Where(item => (item.QCTests ?? [])
                            .Any(q => batchNumbers.Contains(q.QCTest.Number ?? 0))).ToList();
                    }

                    if(validityStartDate != null && validityStartDate != null) {
                        results = results.Where(
                            item => item.AnalysisDeadline >= validityStartDate &&
                            item.AnalysisDeadline <= validityEndDate).ToList();
                    }

                    if(executionStartDate != null && executionStartDate != null) {
                        results = results.Where(
                            item => item.CurrentStatus?.ExecuteDateTime >= executionStartDate &&
                            item.CurrentStatus.ExecuteDateTime <= executionEndDate).ToList();
                    }

                    if(conclusionStartDate != null && conclusionStartDate != null) {
                        results = results.Where(
                            item => item.Conclusion >= conclusionStartDate &&
                            item.Conclusion <= conclusionEndDate).ToList();
                    }

                    if(receiptStartDate != null && receiptStartDate != null) {
                        results = results.Where(
                            item => item.Sample?.ReceivedTime >= receiptStartDate &&
                            item.Sample.ReceivedTime <= receiptEndDate).ToList();
                    }

                    if(startStartDate != null && startStartDate != null) {
                        results = results.Where(
                            item => item.CurrentStatus?.StartDateTime >= startStartDate &&
                            item.CurrentStatus.StartDateTime <= startEndDate).ToList();
                    }

                    if(collectStartDate != null && collectStartDate != null) {
                        results = results.Where(
                            item => item.Sample?.TakenDateTime >= collectStartDate &&
                            item.Sample.TakenDateTime <= collectEndDate).ToList();
                    }
                    
                    var samples = results.Select(item => new SampleDTO
                    {
                        Id = item.Sample?.Id,
                        Identification = item.Sample?.Identification,
                        Conclusion = item.Sample?.Conclusion,
                        TakenDateTime = item.Sample?.TakenDateTime,
                        ReceivedTime = item.Sample?.ReceivedTime,
                        CurrentStatus = new Entities.CurrentStatus
                        {
                            Id = item.Sample?.CurrentStatus?.Id,
                            SampleStatus = new Entities.SampleStatus
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
                        SampleType = new Entities.SampleType
                        {
                            Id = item.Sample?.SampleType?.Id,
                            Identification = item.Sample?.SampleType?.Identification
                        },
                        Method = new Method
                        {
                            MasterId = item.Method?.MasterId,
                            Id = item.Method?.Id,
                            Identification = item.Method?.Identification
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
            [FromQuery] int[] methodIds,
            [FromQuery] int[] stageIds,
            [FromQuery] int[] sampleTypeIds,
            [FromQuery] int[] batchNumbers,
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

                if(!methodIds.IsNullOrEmpty()) {
                    results = results.Where(item => methodIds.ToList()
                        .Contains(item.Method?.MasterId ?? 0)).ToList();
                }

                if(!stageIds.IsNullOrEmpty()) {
                    results = results.Where(item => stageIds.ToList()
                        .Contains(item.CurrentStatus?.MethodStatus?.Id ?? 0)).ToList();
                }

                if(!sampleTypeIds.IsNullOrEmpty()) {
                    results = results.Where(item => sampleTypeIds.ToList()
                        .Contains(item?.Sample?.SampleType?.Id ?? 0)).ToList();
                }

                if(!batchNumbers.IsNullOrEmpty()) {
                    results = results.Where(item => (item.QCTests ?? [])
                        .Any(q => batchNumbers.Contains(q.QCTest.Number ?? 0))).ToList();
                }

                if(validityStartDate != null && validityStartDate != null) {
                    results = results.Where(
                        item => item.AnalysisDeadline >= validityStartDate &&
                        item.AnalysisDeadline <= validityEndDate).ToList();
                }

                if(executionStartDate != null && executionStartDate != null) {
                    results = results.Where(
                        item => item.CurrentStatus?.ExecuteDateTime >= executionStartDate &&
                        item.CurrentStatus.ExecuteDateTime <= executionEndDate).ToList();
                }

                if(conclusionStartDate != null && conclusionStartDate != null) {
                    results = results.Where(
                        item => item.Conclusion >= conclusionStartDate &&
                        item.Conclusion <= conclusionEndDate).ToList();
                }

                if(receiptStartDate != null && receiptStartDate != null) {
                    results = results.Where(
                        item => item.Sample?.ReceivedTime >= receiptStartDate &&
                        item.Sample.ReceivedTime <= receiptEndDate).ToList();
                }

                if(startStartDate != null && startStartDate != null) {
                    results = results.Where(
                        item => item.CurrentStatus?.StartDateTime >= startStartDate &&
                        item.CurrentStatus.StartDateTime <= startEndDate).ToList();
                }

                if(collectStartDate != null && collectStartDate != null) {
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
    }
}