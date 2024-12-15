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
            _logger.LogInformation($"Creating account for {req.UserName}");

            var account = _mapper.Map(req);
            
            if (_accountService.GetAccount(account.UserName) != null)
            {
                _logger.LogWarning($"User {req.UserName} already exists");
                return BadRequest("User already exists");
            }

            var newUser = _accountService.CreateAccount(account);

            return Created(nameof(SignUp), new { id = newUser.Id });
        }

        /// <summary>
        ///  user login
        /// </summary>
        /// <response code="400">Model validation error</response>
        /// <response code="500">System error</response>
        [HttpPost("Login")]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto req)
        {
            if (req == null || string.IsNullOrWhiteSpace(req.UserName) || string.IsNullOrWhiteSpace(req.Password))
            {
                _logger.LogWarning("Invalid login request");
                return BadRequest("Invalid request. Username and password are required.");
            }

            _logger.LogInformation($"Login attempt for user: {req.UserName}");

            var account = _accountService.GetAccount(req.UserName!);
            if (account == null)
            {
                _logger.LogWarning($"User {req.UserName} not found");
                return BadRequest("Invalid username or password");
            }

            var isPasswordValid = _accountService.VerifyPasswordHash(req.Password, account.PasswordHash, account.PasswordSalt);
            if (!isPasswordValid)
            {
                _logger.LogWarning($"Invalid password for user: {req.UserName}");
                return BadRequest("Invalid username or password");
            }

            _logger.LogInformation($"User {req.UserName} successfully logged in");

            var role = _roleService.GetRoleById(account.RoleId);

            try
            {
                var jwt = _jwtService.GetJwtToken(account, role);

                Response.Cookies.Append("jwtToken", jwt, new CookieOptions
                {
                    HttpOnly = false, // Prevent JavaScript access
                    Secure = true,   // Only send over HTTPS
                    SameSite = SameSiteMode.None, // Allow cross-site cookies
                    Expires = DateTime.UtcNow.AddHours(3), // Token expiration
                    Path = "/" // Allow all requests to read the cookie
                });

                return Ok(account.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating JWT for user: {UserName}", req.UserName);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// delete account
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAccount(Guid id)
        {
            _logger.LogInformation($"Deleting account with ID: {id}");

            var account = _accountService.GetAccountById(id);
            if (account == null)
            {
                _logger.LogWarning($"Account with ID: {id} not found");
                return NotFound("Account not found");
            }

            _accountService.DeleteAccount(account.Id);

            return NoContent();
        }
    }
}
