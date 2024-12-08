using HumanRegistrationSystem.Entities;
using HumanRegistrationSystem.Repositories;

namespace HumanRegistrationSystem.Services
{
    public interface IPersonService
    {
        Guid CreatePerson(Person person);
    }
    public class PersonService : IPersonService
    {
        private readonly PersonRepository _personRepository;

        public PersonService(PersonRepository personRepository)
        {
            _personRepository = personRepository;
        }
        public PersonService() { }

        public Guid CreatePerson(Person person)
        {
            return _personRepository.Add(person);

        }
    }
}
