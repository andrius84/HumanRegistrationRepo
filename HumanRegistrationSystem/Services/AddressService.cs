using HumanRegistrationSystem.DTOs.Request;
using HumanRegistrationSystem.DTOs.Result;
using HumanRegistrationSystem.Entities;
using HumanRegistrationSystem.Repositories;
using HumanRegistrationSystem.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace HumanRegistrationSystem.Services
{
    public interface IAddressService
    {
        Guid CreateAddress(Address address);
    }
    public class AddressService : IAddressService
    {
        private readonly ILogger<AddressService> _logger;
        private readonly IAddressRepository _addressRepository;

        public AddressService(IAddressRepository addressRepository, ILogger<AddressService> logger)
        {
            _addressRepository = addressRepository;
            _logger = logger;
        }

        public Guid CreateAddress(Address address)
        {
            var addressId = _addressRepository.Add(address);
            return addressId;
        }
    }
}
