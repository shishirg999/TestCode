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
    public class ProviderController : ControllerBase
    {
        private readonly IProviderContext _context;

        public ProviderController(IProviderContext context)
        {
            _context = context;
        }

        [HttpGet("api/userproviders")]
        public ActionResult GetUserProviders()
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var providers = _context.GetAllUserProviders(token);

            if (providers == null)
            {
                return BadRequest();
            }

            return Ok(providers);
        }

        [HttpGet("api/providers/{id}")]
        public ActionResult GetById(int id)
        {
            var provider = _context.GetById(id);

            if (provider == null)
            {
                return BadRequest();
            }

            return Ok(provider);
        }

        [HttpPost("api/providers")]
        public ActionResult Add(Provider provider)
        {
            _context.Add(provider);
            return Ok();
        }

        [HttpPut("api/providers/{id}")]
        public ActionResult Update(int id, Provider provider)
        {
            _context.Update(id, provider);
            return Ok();
        }

        [HttpDelete("api/providers/{id}")]
        public ActionResult Delete(int id)
        {
            _context.Delete(id);
            return Ok();
        }
        
    }
}


// write nunit unit tests for ProviderController.cs and ProviderContext.cs  (ProviderControllerTests.cs and ProviderContextTests.cs):



