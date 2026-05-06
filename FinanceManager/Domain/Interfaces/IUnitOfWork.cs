using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Account> Accounts { get; }
        IRepository<Category> Categories { get; }
        IRepository<Transaction> Transactions { get; }

        // Метод, який фіксує всі зміни в базі даних за один раз
        void Save();
    }
}