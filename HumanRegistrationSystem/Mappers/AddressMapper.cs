using HumanRegistrationSystem.DTOs.Request;
using HumanRegistrationSystem.DTOs.Result;
using HumanRegistrationSystem.Entities;

namespace HumanRegistrationSystem.Mappers
{
    public interface IAddressMapper
    {
        AddressResultDto Map(Address address);
        Address Map(AddressRequestDto addressRequestDto);
    }
    public class AddressMapper : IAddressMapper
    {
        public Address Map(AddressRequestDto addressRequestDto)
        {
            return new Address
            {
                City = addressRequestDto.City,
                Street = addressRequestDto.Street,
                HouseNumber = addressRequestDto.HouseNumber,
                ApartmentNumber = addressRequestDto.ApartmentNumber,
                PersonId = addressRequestDto.PersonId
            };
        }

        public AddressResultDto Map(Address address)
        {
            return new AddressResultDto
            {
                City = address.City,
                Street = address.Street,
                HouseNumber = address.HouseNumber,
                ApartmentNumber = address.ApartmentNumber
            };
        }
    }


}
