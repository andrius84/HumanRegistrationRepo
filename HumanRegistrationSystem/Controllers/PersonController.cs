using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HumanRegistrationSystem.Mappers;
using HumanRegistrationSystem.DTOs.Request;
using HumanRegistrationSystem.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HumanRegistrationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;
        private readonly IPersonMapper _personMapper;
        private readonly IPersonService _personService;

        public PersonController(ILogger<PersonController> logger, IPersonMapper personMapper, IPersonService personService)
        {
            _logger = logger;
            _personMapper = personMapper;
            _personService = personService;
        }

        /// <summary>
        /// Creates a new Person
        /// </summary>
        /// <param name="personRequestDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Post([FromBody] PersonRequestDto personRequestDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid person data received.");
                return BadRequest(new { message = "Gauti neteisingi duomenys" });
            }

            try
            {
                _logger.LogInformation($"Creating a new Person: {personRequestDto.FirstName} {personRequestDto.LastName}");

                var person = _personMapper.Map(personRequestDto);

                _personService.CreatePerson(person);

                _logger.LogInformation($"Person created successfully with ID: {person.Id}");

                return CreatedAtAction(nameof(Post), new { id = person.Id }, new { personId = person.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating person.");
                return StatusCode(500, "An error occurred while creating the person.");
            }
        }

        /// <summary>
        /// Get person data   
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpGet("PersonByAccountId")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Get([FromQuery] Guid accountId)
        {
            _logger.LogInformation($"Getting Person by ID: {accountId}");

            var person = _personService.GetPersonById(accountId);

            if (person == null)
            {
                return NotFound(new { message = "Asmuo nerastas" });
            }

            var personDto = _personMapper.Map(person);

            return Ok(personDto);
        }

        /// <summary>
        /// Updates the FirstName of a person with the given accountId
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="firstName"></param>
        /// <returns></returns>
        [HttpPut("{accountId}/FirstName")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> UpdateFirstName(Guid accountId, [FromBody] string firstName)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (accountId.ToString() != currentUserId)
            {
                return Forbid();
            }

            _logger.LogInformation($"Updating FirstName for AccountId: {accountId}");
            _personService.UpdateFirstName(accountId, firstName);
            
            return NoContent();
        }

        /// <summary>
        /// Updates the LastName of a person with the given accountId
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        [HttpPut("{accountId}/LastName")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> UpdateLastName(Guid accountId, [FromBody] string lastName)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (accountId.ToString() != currentUserId)
            {
                return Forbid();
            }

            _logger.LogInformation($"Updating LastName for AccountId: {accountId}");
            _personService.UpdateLastName(accountId, lastName);
            
            return NoContent();
        }

        /// <summary>
        /// Updates the PersonalCode of a person with the given accountId
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="personalCode"></param>
        /// <returns></returns>
        [HttpPut("{accountId}/personalCode")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> UpdatePersonalCode(Guid accountId, [FromBody] string personalCode)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (accountId.ToString() != currentUserId)
            {
                return Forbid();
            }

            _logger.LogInformation($"Updating Personal Code for AccountId: {accountId}");
            _personService.UpdatePersonalCode(accountId, personalCode);
            
            return NoContent();
        }

        /// <summary>
        /// Updates the Email of a person with the given accountId
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPut("{accountId}/Email")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> UpdateEmail(Guid accountId, [FromBody] string email)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (accountId.ToString() != currentUserId)
            {
                return Forbid();
            }

            _logger.LogInformation($"Updating Email for AccountId: {accountId}");
            _personService.UpdateEmail(accountId, email);
            
            return NoContent();
        }

        /// <summary>
        /// Updates the PhoneNumber of a person with the given accountId
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        [HttpPut("{accountId}/PhoneNumber")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> UpdatePhoneNumber(Guid accountId, [FromBody] string phoneNumber)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (accountId.ToString() != currentUserId)
            {
                return Forbid();
            }

            _logger.LogInformation($"Updating PhoneNumber for AccountId: {accountId}");
            _personService.UpdatePhoneNumber(accountId, phoneNumber);
            
            return NoContent();
        }
    }
}

