using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Repositories
{
    public class BookDBRepository : IBookRepository<Book>
    {
        BookStoreDBContext db;
        public BookDBRepository(BookStoreDBContext _db)
        {
            db = _db;
        }
        public void Add(Book entity)
        {
            db.Books.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var Book = Find(id);
            db.Books.Remove(Book);
            db.SaveChanges();
        }

        public Book Find(int id)
        {
            var Book = db.Books.Include(a=>a.Author).SingleOrDefault(b => b.ID == id);
            return Book;
        }

        public IList<Book> List()
        {
            return db.Books.Include(a=>a.Author).ToList();
        }

        public void Update(int id, Book entity)
        {
            db.Update(entity);
            db.SaveChanges();
        }
        public List<Book> Search(string term)
        {
            return db.Books.Include(a => a.Author).Where(b => b.Title.Contains(term)
                                                      || b.Description.Contains(term)
                                                      || b.Author.FullName.Contains(term)).ToList();
        }
    }
}
