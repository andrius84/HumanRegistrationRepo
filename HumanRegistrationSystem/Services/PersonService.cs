using HumanRegistrationSystem.Entities;
using HumanRegistrationSystem.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HumanRegistrationSystem.Services
{
    public interface IPersonService
    {
        Guid CreatePerson(Person person);
        Person GetPersonById(Guid accountId);
        bool UpdateEmail(Guid accountId, string email);
        bool UpdateFirstName(Guid accountId, string firstName);
        bool UpdateLastName(Guid accountId, string lastName);
        bool UpdatePersonalCode(Guid accountId, string personalCode);
        bool UpdatePhoneNumber(Guid accountId, string phoneNumber);
    }
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;

        public PersonService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public Guid CreatePerson(Person person)
        {
            return _personRepository.Add(person);

        }

        public Person GetPersonById(Guid accountId)
        {
            return _personRepository.GetById(accountId);
        }

        public bool UpdateFirstName(Guid accountId, string firstName)
        {
             _personRepository.UpdateField(accountId, "FirstName", firstName);
            return true;
        }

        public bool UpdateLastName(Guid accountId, string lastName)
        {
             _personRepository.UpdateField(accountId, "LastName", lastName);
            return true;
        }

        public bool UpdatePersonalCode(Guid accountId, string personalCode)
        {
            _personRepository.UpdateField(accountId, "PersonalCode", personalCode);
            return true;
        }

        public bool UpdateEmail(Guid accountId, string email)
        {
             _personRepository.UpdateField(accountId, "Email", email);
            return true;
        }

        public bool UpdatePhoneNumber(Guid accountId, string phoneNumber)
        {
             _personRepository.UpdateField(accountId, "PhoneNumber", phoneNumber);
            return true;
        }
    }
}
