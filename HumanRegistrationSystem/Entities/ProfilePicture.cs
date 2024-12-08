namespace HumanRegistrationSystem.Entities
{
    public class ProfilePicture
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }

        public Guid PersonId { get; set; } = Guid.Empty;
        public Person Person { get; set; } = null!;
    }
}
