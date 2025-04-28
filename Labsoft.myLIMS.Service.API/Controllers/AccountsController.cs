using Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountsService _accountsService;

        public AccountsController(IAccountsService accountsService)
        {
            _accountsService = accountsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAccounts([FromQuery] int accountType)
        {
            var result = await _accountsService.GetAccounts(accountType);

            if (result.Success != null)
            {
                return Ok(result.Success);
            }

            return StatusCode(result.StatusCode, result.Error);
        }
    }
}
