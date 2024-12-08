using HumanRegistrationSystem.Database;
using HumanRegistrationSystem.Entities;
using HumanRegistrationSystem.AdditionalServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HumanRegistrationSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PictureController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly PictureProcessor _imageProcessor;

        public PictureController(ApplicationDbContext context, PictureProcessor imageProcessor)
        {
            _context = context;
            _imageProcessor = imageProcessor;
        }

        //[HttpPost("upload")]
        //public async Task<IActionResult> UploadImage(IFormFile file)
        //{
        //    if (file == null || file.Length == 0)
        //        return BadRequest("No file uploaded.");

        //    using var memoryStream = new MemoryStream();
        //    await file.CopyToAsync(memoryStream);
        //    var originalData = memoryStream.ToArray();

        //    var thumbnailData = _imageProcessor.CreateThumbnail(originalData, width: 200, height: 200);
        //    var thumbnail = new ProfilePicture
        //    {
        //        FileName = file.FileName,
        //        ContentType = file.ContentType,
        //        Data = thumbnailData,
        //    };
        //    _context.ProfilePictures.Add(thumbnail);
        //    await _context.SaveChangesAsync();

        //    return Ok(new { thumbnailId = thumbnail.Id });
        //}

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file, Guid personId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var person = await _context.Persons.FindAsync(personId);
            if (person == null)
                return BadRequest("Person not found.");

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var originalData = memoryStream.ToArray();

            var thumbnailData = _imageProcessor.CreateThumbnail(originalData, width: 200, height: 200);
            var thumbnail = new ProfilePicture
            {
                FileName = file.FileName,
                ContentType = file.ContentType,
                Data = thumbnailData,
                PersonId = personId,
            };

            _context.ProfilePictures.Add(thumbnail);
            await _context.SaveChangesAsync();

            return Ok(new { thumbnailId = thumbnail.Id });
        }

        [HttpGet("picture/{personId}")]
        public async Task<IActionResult> GetImageByPersonId(Guid personId)
        {
            var thumbnail = await _context.ProfilePictures.FirstOrDefaultAsync(pp => pp.PersonId == personId);
            if (thumbnail == null)
                return NotFound(); 

            return File(thumbnail.Data, "image/jpeg");
        }
    }
}
