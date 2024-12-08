using HumanRegistrationSystem.Database;
using HumanRegistrationSystem.Entities;

namespace HumanRegistrationSystem.Repositories
{
    public interface IAccountRepository
    {
        Account Get(string username);
        Guid AddAccount(Account account);
        void Delete(Guid id);
    }

    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _context;

        public AccountRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public Account Get(string userName)
        {
            if (userName == null)
                throw new ArgumentNullException(nameof(userName));

            return _context.Accounts.FirstOrDefault(a => a.UserName == userName);
        }

        public Guid AddAccount(Account account)
        {
            var exists = _context.Accounts.Any(x => x.UserName == account.UserName);
            if (exists)
                throw new ArgumentException("Username already exists");

            _context.Accounts.Add(account);
            _context.SaveChanges();

            return account.Id;
        }

        public bool Exists(Guid id)
        {
            return _context.Accounts.Any(x => x.Id == id);
        }

        public void Delete(Guid id)
        {
            var account = _context.Accounts.Find(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
                _context.SaveChanges();
            }
        }
    }
}
