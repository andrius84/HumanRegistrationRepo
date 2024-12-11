﻿using HumanRegistrationSystem.Entities;
using HumanRegistrationSystem.Database;

namespace HumanRegistrationSystem.Repositories
{
    public interface IPersonRepository
    {
        Guid Add(Person person);
        Person GetById(Guid accountId);
    }

    public class PersonRepository : IPersonRepository
    {
        private readonly ApplicationDbContext _context;

        public PersonRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }
        public PersonRepository() { }

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
    }
}
