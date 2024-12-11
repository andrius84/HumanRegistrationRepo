using HumanRegistrationSystem.Database;
using HumanRegistrationSystem.Entities;

namespace HumanRegistrationSystem.Repositories
{
    public interface IAddressRepository
    {
        Guid Add(Address address);
        Address GetByPersonId(Guid personId);
    }
    public class AddressRepository : IAddressRepository
    {
        private readonly ApplicationDbContext _context;

        public AddressRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Guid Add(Address address)
        {
            _context.Addresses.Add(address);
            _context.SaveChanges();

            return address.Id;
        }

        public Address GetByPersonId(Guid personId)
        {
            return _context.Addresses.FirstOrDefault(x => x.PersonId == personId);
        }
    }


}
