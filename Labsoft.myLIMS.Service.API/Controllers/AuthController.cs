using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Entities;
using Interfaces;
using LabsoftAPI;
using LabsoftAPI.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Services;

namespace Labsoft.myLIMS.Service.API.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController(IAuthServices authServices, IConfiguration configuration, ILogger<AuthController> logger) : ControllerBase
    {
        private readonly IAuthServices _authServices = authServices;
        private readonly IConfiguration _configuration = configuration;
        private readonly ILogger<AuthController> _logger = logger;

        [HttpPost("Login")]
        public async Task<ActionResult<ResponseBase<LoginResponse>>> Login([FromBody] LoginRequest body)
        {
            var response = await _authServices.Login(body.Username, body.Password, body.RefreshToken);

            if(response.StatusCode == 200) {
                response.Success!.AccessToken = GenerateJwtToken(response.Success.AccessToken ?? "", "produto"); //TODO: set company from request
            }
            else {
                LogConfiguration.CreateLogSender(HttpContext.Request, _logger, response.Error?.Exception);
            }

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

        [Authorize]
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

        [HttpPost("LogoutBySessionId")]
        public async Task<ActionResult<ResponseBase<dynamic>>> LogoutBySessionId([FromBody] string sessionId)
        {
            var response = await _authServices.LogoutBySessionId(sessionId);
            
            if(response.StatusCode == 200)
            {
                return StatusCode(response.StatusCode, new ResponseBase<dynamic>
                {
                    Ok = true,
                    Data = response.Success
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

        private string GenerateJwtToken(string identity, string company)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? ""));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("nested_jwt", identity),
                new Claim("nested_company", company),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMonths(12),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
