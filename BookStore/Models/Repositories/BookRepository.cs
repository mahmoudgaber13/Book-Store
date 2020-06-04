using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Repositories
{
    public class BookRepository : IBookRepository<Book>
    {
        List<Book> Books;
        public BookRepository()
        {
            Books = new List<Book>()
            {
                new Book{ID=1 ,Title="C", Description="c description",Author=new Author{ ID=2}, ImgUrl="6.jpg" },
                new Book{ID=2 ,Title="C++" ,Description="c++ description",Author = new Author{ ID=1}, ImgUrl="b.jpg"},
                new Book{ID=3 ,Title="C#" ,Description="c# description",Author = new Author{ ID=3}, ImgUrl="g.jpg" }
            };
            
        }
        public void Add(Book entity)
        {
            entity.ID = Books.Max(b => b.ID)+1;
            Books.Add(entity);
        }

        public void Delete(int id)
        {
            var Book = Find(id);
            Books.Remove(Book);
        }

        public Book Find(int id)
        {
            var Book = Books.SingleOrDefault(b => b.ID == id);
            return Book;
        }

        public IList<Book> List()
        {
            return Books;
        }

        public void Update(int id, Book entity)
        {
            var Book = Find(id);
            Book.ID = entity.ID;
            Book.Description = entity.Description;
            Book.Title = entity.Title;
            Book.Author = entity.Author;
            Book.ImgUrl = entity.ImgUrl;
        }
    }
}
