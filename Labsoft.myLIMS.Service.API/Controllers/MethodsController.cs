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
    public class MethodsController(IMethodsServices methodsServices) : ControllerBase
    {
        private readonly IMethodsServices _methodsServices = methodsServices;

        [HttpGet("{methodId}/AnalysisMethodInstruction")]
        public async Task<ActionResult<ResponseBase<string>>> AnalysisMethodInstruction(int methodId)
        {
            var response = await _methodsServices.AnalysisMethodInstruction(methodId);
            
            if(response.StatusCode == 200)
            {
                var result = response.Success;

                return StatusCode(response.StatusCode, new ResponseBase<string>
                {
                    Data = result
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

        [HttpGet("{methodId}/MethodPrerequisiteAnalysis")]
        public async Task<ActionResult<ResponseBase<List<MethodPrerequisiteAnalysisBasic>>>> MethodPrerequisiteAnalysis(int methodId)
        {
            var response = await _methodsServices.MethodPrerequisiteAnalysis(methodId);
            
            if(response.StatusCode == 200)
            {
                var results = response.Success ?? [];

                return StatusCode(response.StatusCode, new ResponseBase<List<MethodPrerequisiteAnalysisBasic>>
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