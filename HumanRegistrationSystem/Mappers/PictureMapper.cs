using HumanRegistrationSystem.Entities;
using HumanRegistrationSystem.AdditionalServices;
using HumanRegistrationSystem.DTOs.Result;
using HumanRegistrationSystem.DTOs.Request;
using System.Net.Mime;

namespace HumanRegistrationSystem.Mappers
{
    public interface IPictureMapper
    {
        ProfilePicture Map(PictureRequestDto pictureRequestDto);
        PictureResultDto Map(ProfilePicture profilePicture);
    }
    public class PictureMapper : IPictureMapper
    {
        private readonly PictureProcessor _pictureProcessor;
        public PictureMapper(PictureProcessor pictureProcessor)
        {
            _pictureProcessor = pictureProcessor;
        }
        public ProfilePicture Map(PictureRequestDto pictureRequestDto)
        {
            if (pictureRequestDto.Data == null)
            {
                throw new ArgumentException("Data stream cannot be null.", nameof(pictureRequestDto.Data));
            }

            byte[] originalData;
            using (var memoryStream = new MemoryStream())
            {
                pictureRequestDto.Data.CopyToAsync(memoryStream);
                originalData = memoryStream.ToArray();
            }

            var thumbnailData = _pictureProcessor.CreateThumbnail(originalData, 200, 200);

            return new ProfilePicture
            {
                FileName = pictureRequestDto.FileName,
                ContentType = pictureRequestDto.ContentType,
                Data = thumbnailData, 
                PersonId = pictureRequestDto.PersonId
            };
        }

        public PictureResultDto Map(ProfilePicture profilePicture)
        {
            return new PictureResultDto
            {
                ContentType = profilePicture.ContentType,
                Data = profilePicture.Data,
            };
        }
    }

}
