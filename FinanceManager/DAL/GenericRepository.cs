using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class GenericRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly FinanceDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(FinanceDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>(); // Отримуємо доступ до конкретної таблиці
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }
    }
}
