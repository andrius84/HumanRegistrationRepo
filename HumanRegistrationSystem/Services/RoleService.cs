using HumanRegistrationSystem.Repositories;

namespace HumanRegistrationSystem.Services
{
    public interface IRoleService
    {
        void GetRoleById(Guid id);
    }

    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public void GetRoleById(Guid id)
        {
            _roleRepository.GetRoleById(id);
        }
    }
}
