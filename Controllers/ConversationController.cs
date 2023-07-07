using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnboardingApp.Model;
using OnboardingApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Provider = OnboardingApp.Model.Provider;

namespace OnboardingApp.Controllers
{
    [Authorize]
    [ApiController]
    public class ConversationController : ControllerBase
    {
        private readonly IConversationContext _context;

        public ConversationController(IConversationContext context)
        {
            _context = context;
        }

        [HttpPost("api/conversation")]
        public async Task<ActionResult> AddAsync(ConversationInputs conversation)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            if (conversation == null || string.IsNullOrEmpty(token))
            {
                return BadRequest();
            }

            if (conversation.conversations == null)
            {
                throw new ArgumentNullException("Conversation Question Answer not found");
            }
            int conversationId = await _context.SaveConversationAsync(token, conversation.providerId, conversation.conversations);
            if (conversationId <= 0)
            {
                return BadRequest();
            }
            return Ok(conversationId);
        }

        [HttpPost("api/summary")]
        public async Task<ActionResult> UpdateAsync([FromBody] ConversationSummary conversationSummary)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var response = await _context.SaveConversationSummaryAsync(token, conversationSummary.conversationId, conversationSummary.summary);
            if(response == null)
            {
                return BadRequest();
            }
            return Ok(response);
        }
    }
}
