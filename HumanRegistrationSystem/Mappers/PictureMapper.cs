using HumanRegistrationSystem.Entities;
using HumanRegistrationSystem.AdditionalServices;
using HumanRegistrationSystem.DTOs.Result;
using HumanRegistrationSystem.DTOs.Request;
using System.Net.Mime;

namespace HumanRegistrationSystem.Mappers
{
    public interface IPictureMapper
    {
    }
    public class PictureMapper : IPictureMapper
    {
        private readonly PictureProcessor _imageProcessor;
        public PictureMapper(PictureProcessor imageProcessor)
        {
            _imageProcessor = imageProcessor;
        }
        public ProfilePicture Map(PictureRequestDto pictureRequestDto)
        {
            using var memoryStream = new MemoryStream();
            pictureRequestDto.Data.CopyToAsync(memoryStream);
            var originalData = memoryStream.ToArray();
            var thumbnailData = _imageProcessor.CreateThumbnail(originalData, 200, 200);

            return new ProfilePicture
            {
                ContentType = pictureRequestDto.Data.ContentType,
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
