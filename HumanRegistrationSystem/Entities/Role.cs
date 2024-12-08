namespace HumanRegistrationSystem.Entities
{
    public class Role
    {
        public int Id { get; set; } 
        public string Name { get; set; } = null!;

        public List<Account> Accounts { get; set; } = new List<Account>();
    }
}
