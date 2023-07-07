using Microsoft.AspNetCore.Mvc;
using OnboardingApp.Model;
using OnboardingApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingApp.Controllers
{
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginContext _context;

        public LoginController(ILoginContext context)
        {
            _context = context;
        }


        [HttpPost("api/login")]
        public ActionResult Login(LoginRequest loginRequest)
        {
            var response = _context.Login(loginRequest);

            if (response.StatusCode == 200)
                return Ok(response.Token);
            return BadRequest(response.StatusMessage);
        }

        [HttpPost("api/logout")]
        public ActionResult Logout()
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            _context.Logout(token);
            return Ok();
        
        }

        [HttpPost("api/register")]
        public ActionResult Register(RegisterRequest registerRequest)
        {
            var response = _context.Register(registerRequest);

            if (response.StatusCode == 200)
                return Ok(response.Token);
            return BadRequest(response.StatusMessage);
        }
        
    }
}