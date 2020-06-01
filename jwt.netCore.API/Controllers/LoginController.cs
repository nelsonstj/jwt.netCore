using jwt.netCore.API.Models;
using jwt.netCore.API.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace jwt.netCore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class LoginController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public LoginController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] LoginUser loginUser)
        {
            var auth = _authenticationService.Authenticate(loginUser);
            if (!auth.Result.Success)
                return Ok(auth.Result);

            return Ok(auth.Result);
        }
    }
}