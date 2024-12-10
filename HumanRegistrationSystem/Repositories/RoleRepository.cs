using HumanRegistrationSystem.Database;
using HumanRegistrationSystem.Entities;

namespace HumanRegistrationSystem.Repositories
{
    public interface IRoleRepository
    {
        Role GetRoleById(int id);
    }

    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Role GetRoleById(int id)
        {
            return _context.Roles.FirstOrDefault(r => r.Id == id);
        }
    }
}
