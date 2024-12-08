﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HumanRegistrationSystem.Mappers;
using HumanRegistrationSystem.DTOs.Request;
using HumanRegistrationSystem.Services;

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
        public async Task<IActionResult> Post([FromBody] AddressRequestDto addressRequestDto)
        {
            _logger.LogInformation($"Creating a new Address {addressRequestDto.City} {addressRequestDto.Street}");

            var address = _addressMapper.Map(addressRequestDto);

            _addressService.CreateAddress(address);

            return Created(nameof(Post), new { id = address.Id });
        }
    }
}
