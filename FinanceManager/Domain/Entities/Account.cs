namespace Domain.Entities
{
    public class Account : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}