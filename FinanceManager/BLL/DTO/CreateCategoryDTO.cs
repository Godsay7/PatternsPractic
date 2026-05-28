using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace BLL.DTO
{
    public class CreateCategoryDTO
    {
        public string Name { get; set; } = string.Empty;
        public TransactionType Type { get; set; }
    }
}
