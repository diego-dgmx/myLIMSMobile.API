using Entities;
using LabsoftAPI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Services;

namespace Labsoft.myLIMS.Service.API.Controllers
{

    [Route("api/[controller]")]
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
        public async Task<IActionResult> GetAllMethods()
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

        [HttpGet("GetSamplesSummary")]
        public async Task<IActionResult> GetSamplesSummary([FromQuery] int[] sampleIds, [FromQuery] int[] methodIds)
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
        public async Task<IActionResult> GetSamples(
            [FromQuery] string? sampleType,
            [FromQuery] int[] sampleIds,
            [FromQuery] int[] methodIds,
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
                        ServiceArea = new Entities.ServiceArea
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
        public async Task<IActionResult> GetBatchQCs(
            [FromQuery] int[] sampleIds,
            [FromQuery] int[] methodIds,
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