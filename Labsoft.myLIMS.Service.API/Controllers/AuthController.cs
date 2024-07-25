using Entities;
using LabsoftAPI;
using Microsoft.AspNetCore.Mvc;

namespace Labsoft.myLIMS.Service.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthServices authServices) : ControllerBase
    {
        private readonly IAuthServices _authServices = authServices;

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest body)
        {
            var response = await _authServices.Login(body.Username, body.Password);

            return StatusCode(response.StatusCode, new ResponseBase<dynamic>
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
        public async Task<IActionResult> Me(string email)
        {
            if (Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                if(authorizationHeader.ToString().StartsWith("Bearer "))
                {
                    var token = authorizationHeader.ToString()["Bearer ".Length..].Trim();
                    var response = await _authServices.Me(token, email);

                    return response.StatusCode switch
                    {
                        200 => StatusCode(response.StatusCode, new ResponseBase<MeResponse>
                        {
                            Data = response.Success
                        }),
                        204 => StatusCode(200, new ResponseBase<dynamic>
                        {
                            Message = "no_content"
                        }),
                        401 => Unauthorized(new ResponseBase<dynamic>
                        {
                            Ok = false,
                            Message = "unauthorized_error",
                            Error = new ErrorBase
                            {
                                Code = "unauthorized_error",
                                Description = "expired_auth_token"
                            }
                        }),
                        _ => StatusCode(500, new ResponseBase<dynamic>
                        {
                            Ok = false,
                            Message = response.Error?.ErrorDescription ?? "internal_server_error",
                            Error = new ErrorBase
                            {
                                Code = response.Error?.Error ?? "internal_server_error",
                                Description = response.Error?.ErrorDescription ?? "unknown_error"
                            }
                        }),
                    };
                }
                else {
                    return Unauthorized(new ResponseBase<dynamic>
                    {
                        Ok = false,
                        Message = "unauthorized_error",
                        Error = new ErrorBase
                        {
                            Code = "unauthorized_error",
                            Description = "invalid_auth_token"
                        }
                    });
                }
            }
            else
            {
                return Unauthorized(new ResponseBase<dynamic>
                {
                    Ok = false,
                    Message = "unauthorized_error",
                    Error = new ErrorBase
                    {
                        Code = "unauthorized_error",
                        Description = "auth_token_not_present_in_headers"
                    }
                });
            }
        }
    }

}
