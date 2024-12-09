using HumanRegistrationSystem.Database;
using HumanRegistrationSystem.Entities;
using HumanRegistrationSystem.AdditionalServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace HumanRegistrationSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PictureController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly PictureProcessor _imageProcessor;
        private readonly ILogger<PictureController> _logger;

        public PictureController(ApplicationDbContext context, PictureProcessor imageProcessor, ILogger<PictureController> logger)
        {
            _context = context;
            _imageProcessor = imageProcessor;
            _logger = logger;
        }

        //[HttpPost("upload")]
        ////[Authorize(Roles = "User")]
        //public async Task<IActionResult> UploadImage(IFormFile file, Guid personId)
        //{
        //    if (file == null || file.Length == 0)
        //        return BadRequest("No file uploaded.");

        //    var person = await _context.Persons.FindAsync(personId);
        //    if (person == null)
        //        return BadRequest("Person not found.");

        //    using var memoryStream = new MemoryStream();
        //    await file.CopyToAsync(memoryStream);
        //    var originalData = memoryStream.ToArray();

        //    var thumbnailData = _imageProcessor.CreateThumbnail(originalData, width: 200, height: 200);
        //    var thumbnail = new ProfilePicture
        //    {
        //        FileName = file.FileName,
        //        ContentType = file.ContentType,
        //        Data = thumbnailData,
        //        PersonId = personId,
        //    };

        //    _context.ProfilePictures.Add(thumbnail);
        //    await _context.SaveChangesAsync();

        //    return Ok(new { thumbnailId = thumbnail.Id });
        //}

        [HttpPost("upload")]
        // [Authorize(Roles = "User")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromQuery] Guid personId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            // Find the person by ID
            var person = await _context.Persons.FindAsync(personId);
            if (person == null)
                return NotFound("Person not found.");

            try
            {
                // Read file into memory
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                var originalData = memoryStream.ToArray();

                // Create thumbnail
                var thumbnailData = _imageProcessor.CreateThumbnail(originalData, width: 200, height: 200);

                // Create a new ProfilePicture entity
                var thumbnail = new ProfilePicture
                {
                    FileName = file.FileName,
                    ContentType = file.ContentType,
                    Data = thumbnailData,
                    PersonId = personId,
                };

                // Save to database
                _context.ProfilePictures.Add(thumbnail);
                await _context.SaveChangesAsync();

                // Return success response with the thumbnail ID
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading image for person ID: {PersonId}", personId);
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("picture/{personId}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetImageByPersonId(Guid personId)
        {
            var thumbnail = await _context.ProfilePictures.FirstOrDefaultAsync(pp => pp.PersonId == personId);
            if (thumbnail == null)
                return NotFound(); 

            return File(thumbnail.Data, "image/jpeg");
        }
    }
}
