using HumanRegistrationSystem.Entities;
using HumanRegistrationSystem.Database;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HumanRegistrationSystem.Repositories
{
    public interface IPersonRepository
    {
        Guid Add(Person person);
        Person GetById(Guid accountId);
        void UpdateField(Guid accountId, string fieldName, string fieldValue);
    }

    public class PersonRepository : IPersonRepository
    {
        private readonly ApplicationDbContext _context;

        public PersonRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public Guid Add(Person person)
        {
            _context.Persons.Add(person);
            _context.SaveChanges();

            return person.Id;
        }

        public Person GetById(Guid accountId)
        {
            return _context.Persons.First(x => x.AccountId == accountId);
        }

        public void UpdateField(Guid accountId, string fieldName, string fieldValue)
        {
            var person = _context.Persons.FirstOrDefault(p => p.AccountId == accountId);
            if (person == null)
            {
                throw new KeyNotFoundException($"Person with AccountId {accountId} not found.");
            }

            var propertyInfo = person.GetType().GetProperty(fieldName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (propertyInfo == null)
            {
                throw new ArgumentException($"Field '{fieldName}' does not exist or cannot be updated.");
            }

            propertyInfo.SetValue(person, Convert.ChangeType(fieldValue, propertyInfo.PropertyType));
            _context.SaveChanges();
        }
    }
}
