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

            if(response.Error != null)
            {
                return BadRequest(new ResponseBase<ErrorBase>{
                    Ok = false,
                    Message = response.Error.ErrorDescription,
                    Error = new ErrorBase{
                        Code = response.Error.Error,
                        Description = response.Error.ErrorDescription
                    }
                });
            }
            else {
                return Ok(new ResponseBase<LoginSuccessResponse>{
                    Data = response.Success
                });
            }
        }
    }

}
