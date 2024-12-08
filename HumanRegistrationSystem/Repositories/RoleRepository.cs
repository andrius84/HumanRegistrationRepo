using HumanRegistrationSystem.Database;

namespace HumanRegistrationSystem.Repositories
{
    public interface IRoleRepository
    {
        void GetRoleById(Guid id);
    }

    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void GetRoleById(Guid id)
        {
            _context.Roles.Find(id);
        }
    }
}
