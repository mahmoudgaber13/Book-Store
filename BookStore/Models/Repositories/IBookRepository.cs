using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Repositories
{
    public interface IBookRepository<TEntity>
    {
        IList<TEntity> List();
        void Add(TEntity entity);
        void Delete(int id);
        void Update(int id, TEntity entity);
        TEntity Find(int id);
        public List<TEntity> Search(string term);

    }
}
