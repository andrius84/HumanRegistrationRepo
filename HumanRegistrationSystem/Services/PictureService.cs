using HumanRegistrationSystem.Entities;
using HumanRegistrationSystem.Mappers;
using HumanRegistrationSystem.Repositories;
using Microsoft.AspNetCore.Http;


namespace HumanRegistrationSystem.Services
{
    public interface IPictureService
    {
        ProfilePicture GetPictureByPersonId(Guid personId);
    }
    public class PictureService : IPictureService
    {
        private readonly IPictureMapper _pictureMapper;
        private readonly ILogger<PictureService> _logger;
        private readonly IPictureRepository _context;

        public PictureService(IPictureMapper pictureMapper, ILogger<PictureService> logger, IPictureRepository context)
        {
            _pictureMapper = pictureMapper;
            _logger = logger;
            _context = context;
        }

        public ProfilePicture GetPictureByPersonId(Guid personId)
        {
            return _context.GetByPersonId(personId);
        }

        public void UploadPicture(ProfilePicture profilePicture)
        {
            _context.Add(profilePicture);
        }
    }
}
