using HumanRegistrationSystem.Entities;
using HumanRegistrationSystem.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace HumanRegistrationSystem.Services
{
    public interface IAccountService
    {
        Account CreateAccount(Account account);
        bool Login(string username, string password, out string role);
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        Account GetAccount(string userName);
    }

    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger<AccountService> _logger;

        public AccountService(IAccountRepository accountRepository, ILogger<AccountService> logger)
        {
            _accountRepository = accountRepository;
            _logger = logger;
        }

        public Account CreateAccount(Account account)
        {
            _accountRepository.AddAccount(account);
            return account;
        }

        public bool Login(string userName, string password, out string role)
        {
            var account = _accountRepository.Get(userName);
            role = account.Role.Name;
            if (VerifyPasswordHash(password, account.PasswordHash, account.PasswordSalt))
                return true;

            return false;
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA256();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA256(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            return computedHash.SequenceEqual(passwordHash);
        }

        public Account GetAccount(string userName)
        {
            return _accountRepository.Get(userName);
        }





    }
}
