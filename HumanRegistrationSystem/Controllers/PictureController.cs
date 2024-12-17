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

        /// <summary>
        /// upload image
        /// </summary>
        /// <param name="pictureRequestDto"></param>
        /// <returns></returns>
        [HttpPost("upload/{personId}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> UploadImage([FromForm] PictureRequestDto pictureRequestDto)
        {
            if (pictureRequestDto?.Data == null)
            {
                return BadRequest(new { Message = "Negautas paveikslėlis" });
            }

            var picture = _pictureMapper.Map(pictureRequestDto);
            _pictureService.UploadPicture(picture);

            return Ok();
        }

        /// <summary>
        /// get image by personId
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpGet("{personId}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetImageByPersonId(Guid personId)
        {
            var thumbnail = _pictureService.GetPictureByPersonId(personId);
            if (thumbnail == null || thumbnail.Data == null)
            {
                return NotFound(new { Message = "Nerasta paveikslėlio pagal duotą Id" });
            }

            var thumbnailDto = _pictureMapper.Map(thumbnail);

            return File(thumbnailDto.Data, thumbnailDto.ContentType);
        }
    }
}
