using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FinanceDbContext _context;

        private IRepository<Account>? _accounts;
        private IRepository<Category>? _categories;
        private IRepository<Transaction>? _transactions;

        public UnitOfWork(FinanceDbContext context)
        {
            _context = context;
        }

        public IRepository<Account> Accounts => _accounts ??= new GenericRepository<Account>(_context);
        public IRepository<Category> Categories => _categories ??= new GenericRepository<Category>(_context);
        public IRepository<Transaction> Transactions => _transactions ??= new GenericRepository<Transaction>(_context);

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
