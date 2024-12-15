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
    public class AddressController : ControllerBase
    {
        private readonly ILogger<AddressController> _logger;
        private readonly IAddressMapper _addressMapper;
        private readonly IAddressService _addressService;

        public AddressController(ILogger<AddressController> logger, IAddressMapper addressMapper, IAddressService addressService)
        {
            _logger = logger;
            _addressMapper = addressMapper;
            _addressService = addressService;
        }

        // POST: api/Address
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Post([FromBody] AddressRequestDto addressRequestDto)
        {
            _logger.LogInformation($"Creating a new Address {addressRequestDto.City} {addressRequestDto.Street}");

            var address = _addressMapper.Map(addressRequestDto);

            _addressService.CreateAddress(address);

            return Created(nameof(Post), new { id = address.Id });
        }

        // GET: api/AddressByPersonId
        [HttpGet("AddressByPersonId")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Get([FromQuery] Guid personId)
        {
            _logger.LogInformation($"Getting Address by PersonId {personId}");

            var address = _addressService.GetAddressByPersonId(personId);

            if (address == null)
            {
                return NotFound();
            }

            var addressDto = _addressMapper.Map(address);

            return Ok(addressDto);
        }

        [HttpPut("{personId}/City")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateCity(Guid personId, [FromBody] string city)
        {
            _logger.LogInformation($"Updating city for accountId: {personId}");
            _addressService.UpdateCity(personId, city);
            return NoContent();
        }

        [HttpPut("{personId}/Street")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateStreet(Guid personId, [FromBody] string street)
        {
            _logger.LogInformation($"Updating street for accountId: {personId}");
            _addressService.UpdateStreet(personId, street);
            return NoContent();
        }

        [HttpPut("{personId}/HouseNumber")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateHouseNumber(Guid personId, [FromBody] string houseNumber)
        {
            _logger.LogInformation($"Updating house number for accountId: {personId}");
            _addressService.UpdateHouseNumber(personId, houseNumber);
            return NoContent();
        }

        [HttpPut("{personId}/ApartmentNumber")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateApartmentNumber(Guid personId, [FromBody] string apartmentNumber)
        {
            _logger.LogInformation($"Updating ApartmentNumber for accountId: {personId}");
            _addressService.UpdateApartmentNumber(personId, apartmentNumber);
            return NoContent();
        }
    }
}
