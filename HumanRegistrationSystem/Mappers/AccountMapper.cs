using HumanRegistrationSystem.Entities;
using HumanRegistrationSystem.DTOs.Request;
using HumanRegistrationSystem.Services;
using HumanRegistrationSystem.Entities;

namespace HumanRegistrationSystem.Mappers
{
    public interface IAccountMapper
    {
        Account Map(AccountRequestDto dto);
    }

    public class AccountMapper : IAccountMapper
    {
        private readonly IAccountService _accountService;

        public AccountMapper(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public Account Map(AccountRequestDto dto)
        {
            _accountService.CreatePasswordHash(dto.Password!, out var passwordHash, out var passwordSalt);
            return new Account
            {
                UserName = dto.UserName!,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
            };
        }
    }
}
