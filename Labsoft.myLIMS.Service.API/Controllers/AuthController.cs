using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Entities;
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
    public class AuthController(IAuthServices authServices, IConfiguration configuration) : ControllerBase
    {
        private readonly IAuthServices _authServices = authServices;
        private readonly IConfiguration _configuration = configuration;

        [HttpPost("Login")]
        public async Task<ActionResult<ResponseBase<LoginResponse>>> Login([FromBody] LoginRequest body)
        {
            var response = await _authServices.Login(body.Username, body.Password);

            if(response.StatusCode == 200) {
                response.Success!.AccessToken = GenerateJwtToken(response.Success.AccessToken ?? "");
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

        private string GenerateJwtToken(string identity)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? ""));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, identity),
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
