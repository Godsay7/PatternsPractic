namespace BLL.DTO
{
    public class TransactionDTO
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public string CategoryName { get; set; }
        public string AccountName { get; set; }
        public int AccountId { get; set; }
        public int CategoryId { get; set; }
    }
}
