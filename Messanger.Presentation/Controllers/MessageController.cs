﻿using System.Security.Claims;
using AutoMapper;
using MediatR;
using Messanger.BusinessLogic.Commands.Messages;
using Messanger.BusinessLogic.Queries.Messages.GetMessages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Messanger.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class MessageController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public MessageController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetMessages([FromQuery]GetMessagesRequest getMessagesRequest)
        {
            var getMessagesQuery = _mapper.Map<GetMessagesQuery>(getMessagesRequest);
            
            return Ok(await _mediator.Send(getMessagesQuery));
        }

        [HttpPost("sendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest sendMessageRequest)
        {
            var sendMessageCommand = new SendMessageCommand
            {
                ChatId = sendMessageRequest.ChatId,
                Message = sendMessageRequest.Message
            };


            
            return Ok(await _mediator.Send(sendMessageCommand));
        }
        

    }
}
