using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HumanRegistrationSystem.Mappers;
using HumanRegistrationSystem.DTOs.Request;
using HumanRegistrationSystem.Services;

namespace HumanRegistrationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;
        private readonly IPersonMapper _personMapper;
        private readonly IPersonService _personService;

        public PersonController(ILogger<PersonController> logger, IPersonMapper personMapper)
        {
            _logger = logger;
            _personMapper = personMapper;
        }

        // POST: api/Person
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PersonRequestDto personRequestDto)
        {
            _logger.LogInformation($"Creating a new Person {personRequestDto.FirstName} {personRequestDto.LastName}");
            // Map the PersonRequestDto to a Person entity
            var person = _personMapper.Map(personRequestDto);

            _personService.CreatePerson(person);

            return Created(nameof(Post), new { id = person.Id });
        }
    }
}
