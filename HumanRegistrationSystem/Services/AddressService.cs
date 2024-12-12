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
        Address GetAddressByPersonId(Guid personId);
        bool UpdateApartmentNumber(Guid accountId, string apartmentNumber);
        bool UpdateCity(Guid accountId, string city);
        bool UpdateHouseNumber(Guid accountId, string houseNumber);
        bool UpdateStreet(Guid accountId, string street);
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

        public Address GetAddressByPersonId(Guid personId)
        {
            return _addressRepository.GetByPersonId(personId);
        }

        public bool UpdateCity(Guid accountId, string city)
        {
            _addressRepository.UpdateField(accountId, "City", city);
            return true;
        }

        public bool UpdateStreet(Guid accountId, string street)
        {
            _addressRepository.UpdateField(accountId, "Street", street);
            return true;
        }

        public bool UpdateHouseNumber(Guid accountId, string houseNumber)
        {
            _addressRepository.UpdateField(accountId, "HouseNumber", houseNumber);
            return true;
        }

        public bool UpdateApartmentNumber(Guid accountId, string apartmentNumber)
        {
            _addressRepository.UpdateField(accountId, "ApartmentNumber", apartmentNumber);
            return true;
        }

    }
}
