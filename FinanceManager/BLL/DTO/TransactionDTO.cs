namespace BLL.DTO
{
    public class TransactionDTO
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string CategoryName { get; set; } // Тільки ім'я для виводу на екран
        public string AccountName { get; set; }
        public int AccountId { get; set; }       // ID для вибору зі списку
        public int CategoryId { get; set; }
    }
}
