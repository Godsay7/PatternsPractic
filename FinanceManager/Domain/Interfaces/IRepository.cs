using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        IEnumerable<T> Find(Func<T, bool> predicate);

        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}
