using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Repositories
{
    public class AuthorDBRepository : IBookRepository<Author>
    {
        BookStoreDBContext db;
        public AuthorDBRepository(BookStoreDBContext _db)
        {
            db = _db;
        }
        public void Add(Author entity)
        {
            db.Authors.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var Author = Find(id);
            db.Authors.Remove(Author);
            db.SaveChanges();
        }

        public Author Find(int id)
        {
            var Author = db.Authors.SingleOrDefault(a => a.ID == id);
            return Author;
        }

        public IList<Author> List()
        {
            return db.Authors.ToList();
        }

        public List<Author> Search(string term)
        {
            return db.Authors.Where(a=>a.FullName.Contains(term)).ToList();
        }

        public void Update(int id, Author entity)
        {
            db.Update(entity);
            db.SaveChanges();
        }
    }
}
