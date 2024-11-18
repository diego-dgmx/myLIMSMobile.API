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
    public class QCTestsController(IQCTestsServices qcTestsServices) : ControllerBase
    {
        private readonly IQCTestsServices _qcTestsServices = qcTestsServices;

        private enum BatchSortParam
        {
            batchNumber,
            validity,
            identification,
            started
        }

        [HttpGet]
        public async Task<ActionResult<ResponseBase<Pagination<QCTest>>>> GetQCTests(
            [FromQuery] string? sortParam,
            [FromQuery] string[] batchNumbers,
            [FromQuery] string[] batchIdentifications,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] int perPage = 10,
            [FromQuery] int page = 1)
        {
            var identityCenterToken = User.Claims.FirstOrDefault(c => c.Type == "nested_jwt")?.Value;
            var response = await _qcTestsServices.GetQCTests(identityCenterToken);
            
            if(response.StatusCode == 200)
            {
                var results = response.Success ?? [];

                if (Enum.TryParse<BatchSortParam>(sortParam, out var param)) {
                    switch(param) {
                        case BatchSortParam.batchNumber:
                            results = [.. results.OrderBy(r => r.ControlNumber)];
                            break;
                        case BatchSortParam.validity:
                            results = [.. results.OrderBy(r => r.Expires)];
                            break;
                        case BatchSortParam.identification:
                            results = [.. results.OrderBy(r => r.Identification)];
                            break;
                        case BatchSortParam.started:
                            results = [.. results.OrderBy(r => r.Started)];
                            break;
                    }
                }

                if(!batchNumbers.IsNullOrEmpty()) {
                    results = results.Where(item => batchNumbers.ToList()
                        .Any(number => (item.ControlNumber ?? "")
                            .Contains(number, StringComparison.OrdinalIgnoreCase)))
                                .ToList();
                }

                if(!batchIdentifications.IsNullOrEmpty()) {
                    results = results.Where(item => batchIdentifications.ToList()
                        .Any(identification => (item.Identification ?? "")
                            .Contains(identification, StringComparison.OrdinalIgnoreCase)))
                                .ToList();
                }

                if(endDate != null) {
                    results = results
                        .Where(item => item.Expires?.ToString()[..10] == endDate?.ToString()[..10])
                        .ToList();
                }

                if(startDate != null) {
                    results = results
                        .Where(item => item.Started?.ToString()[..10] == startDate?.ToString()[..10])
                        .ToList();
                }

                return StatusCode(response.StatusCode, new ResponseBase<Pagination<QCTest>>
                {
                    Data = new Pagination<QCTest>
                    {
                        CurrentPage = page,
                        PerPage = perPage,
                        TotalPages = (int) Math.Ceiling((double) results.Count / perPage),
                        TotalItems = results.Count,
                        Items = results.Skip((page - 1) * perPage)
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

        [HttpGet("GetAvailableByQCRoutineBatchId")]
        public async Task<ActionResult<ResponseBase<List<QCTest>>>> GetAvailableByQCRoutineBatchId([FromQuery] int id)
        {
            var identityCenterToken = User.Claims.FirstOrDefault(c => c.Type == "nested_jwt")?.Value;
            var response = await _qcTestsServices.GetAvailableByQCRoutineBatchId(id, identityCenterToken);
            
            if(response.StatusCode == 200)
            {
                var results = response.Success ?? [];

                return StatusCode(response.StatusCode, new ResponseBase<List<QCTest>>
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
        
        [HttpGet("GetLinkedSamplesByQCTestId")]
        public async Task<ActionResult<ResponseBase<List<QCTestLink>>>> GetLinkedSamplesByQCTestId([FromQuery] int id)
        {
            var identityCenterToken = User.Claims.FirstOrDefault(c => c.Type == "nested_jwt")?.Value;
            var response = await _qcTestsServices.GetLinkedSamplesByQCTestId(id, identityCenterToken);
            
            if(response.StatusCode == 200)
            {
                var results = response.Success ?? [];

                return StatusCode(response.StatusCode, new ResponseBase<List<QCTestLink>>
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

        [HttpGet("GetControlSamplesByQCTestId")]
        public async Task<ActionResult<ResponseBase<List<QCTestLink>>>> GetControlSamplesByQCTestId([FromQuery] int id)
        {
            var identityCenterToken = User.Claims.FirstOrDefault(c => c.Type == "nested_jwt")?.Value;
            var response = await _qcTestsServices.GetControlSamplesByQCTestId(id, identityCenterToken);
            
            if(response.StatusCode == 200)
            {
                var results = response.Success ?? [];

                return StatusCode(response.StatusCode, new ResponseBase<List<QCTestLink>>
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
    }
}