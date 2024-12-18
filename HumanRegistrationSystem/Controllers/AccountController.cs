using HumanRegistrationSystem.DTOs.Request;
using HumanRegistrationSystem.Services;
using HumanRegistrationSystem.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SignUp(AccountRequestDto req)
        {
            _logger.LogInformation($"Creating account for {req.UserName}");

            var account = _mapper.Map(req);
            
            if (_accountService.GetAccount(account.UserName) != null)
            {
                _logger.LogWarning($"User {req.UserName} already exists");
                return BadRequest(new { message = "Toks vartotojas jau egzistuoja" });
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto req)
        {
            _logger.LogInformation($"Login attempt for user: {req.UserName}");

            var account = _accountService.GetAccount(req.UserName!);
            if (account == null)
            {
                _logger.LogWarning($"User {req.UserName} not found");
                return BadRequest(new { message = "Vartotojas nerastas" });
            }

            var isPasswordValid = _accountService.VerifyPasswordHash(req.Password, account.PasswordHash, account.PasswordSalt);
            if (!isPasswordValid)
            {
                _logger.LogWarning($"Invalid password for user: {req.UserName}");
                return BadRequest(new { message = "Neteisingas slaptažodis" });
            }


            _logger.LogInformation($"User {req.UserName} successfully logged in");

            var role = _roleService.GetRoleById(account.RoleId);

            try
            {
                var jwt = _jwtService.GetJwtToken(account, role);

                Response.Cookies.Append("jwtToken", jwt, new CookieOptions
                {
                    HttpOnly = false,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddHours(3),
                    Path = "/"
                });

                return Ok(account.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating JWT for user: {UserName}", req.UserName);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// delete account
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{accountId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAccount(Guid accountId)
        {
            _logger.LogInformation($"Deleting account with ID: {accountId}");

            var account = _accountService.GetAccountById(accountId);
            if (account == null)
            {
                _logger.LogWarning($"Account with ID: {accountId} not found");
                return BadRequest(new { message = "Vartotojas nerastas" });
            }

            _accountService.DeleteAccount(account.Id);

            return NoContent();
        }
    }
}
