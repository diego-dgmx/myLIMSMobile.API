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
    public class MessagesController(IMessagesServices messagesServices) : ControllerBase
    {
        private readonly IMessagesServices _messagesServices = messagesServices;

        [HttpGet("{sampleId}/BySampleId")]
        public async Task<ActionResult<ResponseBase<List<MessageBasic>>>> BySampleId(int sampleId)
        {
            var response = await _messagesServices.GetMessagesBySampleId(sampleId);
            
            if(response.StatusCode == 200)
            {
                var results = response.Success ?? [];

                return StatusCode(response.StatusCode, new ResponseBase<List<MessageBasic>>
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

        [HttpGet("GetMessageTypes")]
        public async Task<ActionResult<ResponseBase<List<MessageTypeBasic>>>> GetMessageTypes()
        {
            var response = await _messagesServices.GetMessageTypes();
            
            if(response.StatusCode == 200)
            {
                var results = response.Success ?? [];

                return StatusCode(response.StatusCode, new ResponseBase<List<MessageTypeBasic>>
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