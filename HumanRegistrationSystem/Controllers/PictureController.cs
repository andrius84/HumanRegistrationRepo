using HumanRegistrationSystem.Database;
using HumanRegistrationSystem.Entities;
using HumanRegistrationSystem.AdditionalServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using HumanRegistrationSystem.Mappers;
using HumanRegistrationSystem.Services;
using HumanRegistrationSystem.DTOs.Request;

namespace HumanRegistrationSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PictureController : ControllerBase
    {
        private readonly IPictureMapper _pictureMapper;
        private readonly IPictureService _pictureService;
        private readonly ILogger<PictureController> _logger;

        public PictureController(IPictureMapper pictureMapper, IPictureService pictureService, ILogger<PictureController> logger)
        {
            _pictureMapper = pictureMapper;
            _pictureService = pictureService;
            _logger = logger;
        }

        //[HttpPost("upload")]
        //[Authorize(Roles = "User,Admin")]
        //public async Task<IActionResult> UploadImage([FromBody] PictureRequestDto pictureRequestDto)
        //{
        //    if (pictureRequestDto.Data == null)
        //        return BadRequest("No file uploaded.");

        //    var thumbnail = _pictureMapper.Map(pictureRequestDto);

        //    _pictureService.UploadPicture(thumbnail);

        //    return Ok();           
        //}

        [HttpPost("upload/{personId}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> UploadImage([FromForm] PictureRequestDto pictureRequestDto)
        {
            if (pictureRequestDto?.Data == null)
            {
                return BadRequest("No file uploaded.");
            }

            var picture = _pictureMapper.Map(pictureRequestDto);
            _pictureService.UploadPicture(picture);

            return Ok();
        }

        [HttpGet("{personId}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetImageByPersonId(Guid personId)
        {
            var thumbnail = _pictureService.GetPictureByPersonId(personId);
            if (thumbnail == null || thumbnail.Data == null)
            {
                return NotFound(new { Message = "Profile picture not found for the given person ID." });
            }

            var thumbnailDto = _pictureMapper.Map(thumbnail);

            //var mimeType = "image/jpeg"; // You can also get the mime type from the file extension

            return File(thumbnailDto.Data, thumbnailDto.ContentType);
        }

        private byte[] ConvertStreamToByteArray(Stream stream)
        {
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
