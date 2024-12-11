using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HumanRegistrationSystem.Mappers;
using HumanRegistrationSystem.DTOs.Request;
using HumanRegistrationSystem.Services;
using Microsoft.AspNetCore.Authorization;

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

        // POST: api/Person
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Post([FromBody] PersonRequestDto personRequestDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid person data received.");
                return BadRequest(ModelState); // Return detailed validation errors
            }

            try
            {
                _logger.LogInformation($"Creating a new Person: {personRequestDto.FirstName} {personRequestDto.LastName}");

                // Map DTO to entity
                var person = _personMapper.Map(personRequestDto);

                // Create person
                _personService.CreatePerson(person);

                // Log success
                _logger.LogInformation($"Person created successfully with ID: {person.Id}");

                // Return Created response with the person's ID
                return CreatedAtAction(nameof(Post), new { id = person.Id }, new { personId = person.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating person.");
                return StatusCode(500, "An error occurred while creating the person.");
            }
        }

        // GET: api/PersonById
        [HttpGet("PersonById")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Get([FromQuery] Guid accountId)
        {
            _logger.LogInformation($"Getting Person by ID: {accountId}");

            var person = _personService.GetPersonById(accountId);

            if (person == null)
            {
                return NotFound();
            }

            var personDto = _personMapper.Map(person);

            return Ok(personDto);
        }
    }
}
