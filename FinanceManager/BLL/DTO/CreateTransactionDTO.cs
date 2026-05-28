using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class CreateTransactionDTO
    {
        public int AccountId { get; set; }
        public int CategoryId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
    }
}
