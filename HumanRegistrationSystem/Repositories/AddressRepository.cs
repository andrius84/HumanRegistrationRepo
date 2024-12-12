using HumanRegistrationSystem.Database;
using HumanRegistrationSystem.Entities;
using System.Reflection;

namespace HumanRegistrationSystem.Repositories
{
    public interface IAddressRepository
    {
        Guid Add(Address address);
        Address GetByPersonId(Guid personId);
        void UpdateField(Guid personId, string fieldName, string fieldValue);
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

        public void UpdateField(Guid personId, string fieldName, string fieldValue)
        {
            var address = _context.Addresses.FirstOrDefault(p => p.PersonId == personId);
            if (address == null)
            {
                throw new KeyNotFoundException($"Person with AccountId {personId} not found.");
            }

            var propertyInfo = address.GetType().GetProperty(fieldName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (propertyInfo == null)
            {
                throw new ArgumentException($"Field '{fieldName}' does not exist or cannot be updated.");
            }

            propertyInfo.SetValue(address, Convert.ChangeType(fieldValue, propertyInfo.PropertyType));
            _context.SaveChanges();
        }
    }
}
