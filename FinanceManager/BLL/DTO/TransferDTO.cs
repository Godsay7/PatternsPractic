using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class TransferDTO
    {
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public decimal Amount { get; set; }
        public int ExpenseCategoryId { get; set; } = 6;
        public int IncomeCategoryId { get; set; } = 5;
        public string? Description { get; set; }
    }
}
