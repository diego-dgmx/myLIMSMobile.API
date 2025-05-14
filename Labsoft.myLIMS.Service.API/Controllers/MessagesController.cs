using Entities;
using Interfaces;
using LabsoftAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Labsoft.myLIMS.Service.API.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MessagesController(IMessagesServices messagesServices, ILogger<MessagesController> logger) : ControllerBase
    {
        private readonly IMessagesServices _messagesServices = messagesServices;
        private readonly ILogger<MessagesController> _logger = logger;

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

        [HttpPost("SendMessageWithEntitiesAttached")]
        public async Task<ActionResult<ResponseBase<dynamic>>> SendMessageWithEntitiesAttached(
            [FromBody] SendMessageWithEntitiesAttachedDTO body)
        {
            var identityCenterToken = User.Claims.FirstOrDefault(c => c.Type == "nested_jwt")?.Value;
            var identityCompany = User.Claims.FirstOrDefault(c => c.Type == "nested_company")?.Value;
            var response = await _messagesServices.SendMessage(identityCenterToken, identityCompany, body);
            
            if(response.StatusCode == 201)
            {
                var results = response.Success;

                return StatusCode(response.StatusCode, new ResponseBase<dynamic>
                {
                    Data = results
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

        [HttpPut("UpdateActiveStatus")]
        public async Task<ActionResult<ResponseBase<dynamic>>> UpdateActiveStatus(
            [FromBody] UpdateMessageActiveStatusDTO body)
        {
            var response = await _messagesServices.UpdateActiveStatus(body);

            if(response.StatusCode == 200)
            {
                var results = response.Success;

                return StatusCode(response.StatusCode, new ResponseBase<dynamic>
                {
                    Data = results
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
    }
}