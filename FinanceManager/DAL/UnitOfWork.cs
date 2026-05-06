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

        // Приватні поля для зберігання створених репозиторіїв
        private IRepository<Account>? _accounts;
        private IRepository<Category>? _categories;
        private IRepository<Transaction>? _transactions;

        public UnitOfWork(FinanceDbContext context)
        {
            _context = context;
        }

        // Якщо репозиторій ще не створено - створюємо, якщо створено - повертаємо існуючий
        public IRepository<Account> Accounts => _accounts ??= new GenericRepository<Account>(_context);
        public IRepository<Category> Categories => _categories ??= new GenericRepository<Category>(_context);
        public IRepository<Transaction> Transactions => _transactions ??= new GenericRepository<Transaction>(_context);

        public void Save()
        {
            _context.SaveChanges(); // Єдине місце, де ми звертаємося до бази для запису!
        }

        // Обов'язкове звільнення ресурсів підключення до БД
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
