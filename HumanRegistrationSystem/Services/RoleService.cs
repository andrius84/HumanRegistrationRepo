using HumanRegistrationSystem.Repositories;
using HumanRegistrationSystem.Entities;

namespace HumanRegistrationSystem.Services
{
    public interface IRoleService
    {
        Role GetRoleById(int id);
    }

    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public Role GetRoleById(int id)
        {
            return _roleRepository.GetRoleById(id);
        }
    }
}
