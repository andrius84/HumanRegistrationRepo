namespace HumanRegistrationSystem.Entities
{
    public class ProfilePicture
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }

        // Foreign key
        public Guid PersonId { get; set; } = Guid.Empty;

        // Navigation property
        public Person Person { get; set; } = null!;
    }
}
