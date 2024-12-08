using HumanRegistrationSystem.DTOs.Request;
using HumanRegistrationSystem.Services;
using HumanRegistrationSystem.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace HumanRegistrationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AccountController> _logger;
        private readonly IRoleService _roleService;
        private readonly IAccountMapper _mapper;

        public AccountController(IAccountService accountService, IJwtService jwtService, ILogger<AccountController> logger, IRoleService roleService, IAccountMapper mapper)
        {
            _accountService = accountService;
            _jwtService = jwtService;
            _logger = logger;
            _roleService = roleService;
            _mapper = mapper;
        }

        /// <summary>
        /// user sign up
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(AccountRequestDto req)
        {
            var account = _mapper.Map(req);
            var userId = _accountService.CreateAccount(account);

            return Created("", new { id = userId });
        }

        /// <summary>
        ///  user login
        /// </summary>
        /// <response code="400">Model validation error</response>
        /// <response code="500">System error</response>
        [HttpPost("Login")]
        [Produces(MediaTypeNames.Text.Plain)]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(LoginRequestDto req)
        {
            _logger.LogInformation($"Login attempt for {req.UserName}");

            // Retrieve the account by username
            var account = _accountService.GetAccount(req.UserName!);
            if (account == null)
            {
                _logger.LogWarning($"User {req.UserName} not found");
                return BadRequest("User not found");
            }

            // Verify password
            var isPasswordValid = _accountService.VerifyPasswordHash(req.Password, account.PasswordHash, account.PasswordSalt);
            if (!isPasswordValid)
            {
                _logger.LogWarning($"Invalid password for {req.UserName}");
                return BadRequest("Invalid username or password");
            }

            // Log successful login
            _logger.LogInformation($"User {req.UserName} successfully logged in");

            // Generate the JWT token
            var jwt = _jwtService.GetJwtToken(account);

            // Set the JWT in an HttpOnly cookie with Secure and SameSite options
            Response.Cookies.Append("jwtToken", jwt, new CookieOptions
            {
                HttpOnly = true,   // Prevent access to cookie via JavaScript
                Secure = true,     // Ensure cookie is only sent over HTTPS
                SameSite = SameSiteMode.Strict,  // Prevent CSRF attacks
                Expires = DateTime.UtcNow.AddHours(1)  // Set token expiration time
            });

            // Return a successful response, without sending the token back in the body
            return Ok("Login successful");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get()
        {
            return Ok("Hello from secure API");
        }

    }
}
