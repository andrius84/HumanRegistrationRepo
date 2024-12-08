using HumanRegistrationSystem.DTOs.Result;
using HumanRegistrationSystem.DTOs.Request;
using HumanRegistrationSystem.Entities;

namespace HumanRegistrationSystem.Mappers
{
    public interface IPersonMapper
    {
        Person Map(PersonRequestDto personRequestDto);
    }
    public class PersonMapper : IPersonMapper
    {
        public Person Map(PersonRequestDto personRequestDto)
        {
            return new Person
            {
                FirstName = personRequestDto.FirstName,
                LastName = personRequestDto.LastName,
                PersonalCode = personRequestDto.PersonalCode,
                PhoneNumber = personRequestDto.PhoneNumber,
                Email = personRequestDto.Email,
            };
        }

        public PersonRequestDto Map(Person person)
        {
            return new PersonRequestDto
            {
                FirstName = person.FirstName,
                LastName = person.LastName,
                PersonalCode = person.PersonalCode,
                PhoneNumber = person.PhoneNumber,
                Email = person.Email,
            };
        }
    }


}
