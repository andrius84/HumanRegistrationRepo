namespace HumanRegistrationSystem.DTOs.Result
{
    public class PictureResultDto
    {
        /// <summary>
        /// Content type of the picture
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Data of the picture
        /// </summary>
        public byte[] Data { get; set; }
    }
}
