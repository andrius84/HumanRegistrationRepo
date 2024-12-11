using HumanRegistrationSystem.DTOs.Result;
using HumanRegistrationSystem.DTOs.Request;
using HumanRegistrationSystem.Entities;

namespace HumanRegistrationSystem.Mappers
{
    public interface IPersonMapper
    {
        Person Map(PersonRequestDto personRequestDto);
        PersonResultDto Map(Person person);
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
                AccountId = personRequestDto.AccountId
            };
        }

        public PersonResultDto Map(Person person)
        {
            return new PersonResultDto
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
