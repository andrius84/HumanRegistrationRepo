using HumanRegistrationSystem.Database;
using HumanRegistrationSystem.Entities;

namespace HumanRegistrationSystem.Repositories
{
    public interface IPictureRepository
    {
        void Add(ProfilePicture profilePicture);
        ProfilePicture GetByPersonId(Guid personId);
    }
    public class PictureRepository : IPictureRepository
    {
        private readonly ApplicationDbContext _context;
        public PictureRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(ProfilePicture profilePicture)
        {
            _context.ProfilePictures.Add(profilePicture);
            _context.SaveChanges();
        }

        public ProfilePicture GetByPersonId(Guid personId)
        {
            return _context.ProfilePictures.FirstOrDefault(pp => pp.PersonId == personId);
        }
    }


}
