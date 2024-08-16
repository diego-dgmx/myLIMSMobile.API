using Entities;
using LabsoftAPI;
using LabsoftAPI.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Services;

namespace Labsoft.myLIMS.Service.API.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController(IAuthServices authServices) : ControllerBase
    {
        private readonly IAuthServices _authServices = authServices;

        [HttpPost("Login")]
        public async Task<ActionResult<ResponseBase<LoginResponse>>> Login([FromBody] LoginRequest body)
        {
            var response = await _authServices.Login(body.Username, body.Password);

            return StatusCode(response.StatusCode, new ResponseBase<LoginResponse>
            {
                Ok = response.Success != null,
                Data = response.Success,
                Message = response.Error?.ErrorDescription,
                Error = response.Error != null ? new ErrorBase
                {
                    Code = response.Error.Error,
                    Description = response.Error.ErrorDescription
                } : null
            });
        }

        [HttpGet("Me/{email}")]
        public async Task<ActionResult<ResponseBase<MeResponse>>> Me(string email)
        {
            var response = await _authServices.Me(email);
            
            if(response.StatusCode == 200)
            {
                var results = response.Success ?? [];

                if(!results.IsNullOrEmpty())
                {
                    return StatusCode(response.StatusCode, new ResponseBase<MeResponse>
                    {
                        Ok = response.Success != null,
                        Data = results[0]
                    });
                }
                else
                {
                    return StatusCode(response.StatusCode, new ResponseBase<dynamic>
                    {
                        Ok = false,
                        Message = "email_not_exists",
                        Error = new ErrorBase
                        {
                            Code = "no_content",
                            Description = "email_not_exists"
                        }
                    });
                }
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
