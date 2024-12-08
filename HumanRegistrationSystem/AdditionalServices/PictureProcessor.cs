using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp;

namespace HumanRegistrationSystem.AdditionalServices
{
    public class PictureProcessor
    {
        public byte[] CreateThumbnail(byte[] imageData, int width, int height)
        {
            using var image = Image.Load(imageData);

            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(width, height),
                Mode = ResizeMode.Stretch
            }));

            using var outputStream = new MemoryStream();
            image.SaveAsJpeg(outputStream);
            return outputStream.ToArray();
        }
    }
}
