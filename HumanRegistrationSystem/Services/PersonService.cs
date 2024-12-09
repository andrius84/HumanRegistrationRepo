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
        private readonly IPersonRepository _personRepository;

        public PersonService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public Guid CreatePerson(Person person)
        {
            return _personRepository.Add(person);

        }
    }
}
