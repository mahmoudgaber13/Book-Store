using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Repositories
{
    public class AuthorRepository : IBookRepository<Author>
    {
        List<Author> Authors;
        public AuthorRepository()
        {
            Authors = new List<Author>()
            {
                new Author{ID=1 ,FullName="Andy Hunt"},
                new Author{ID=2 ,FullName="Martin Fowler"},
                new Author{ID=3 ,FullName="Scott Myers"}
            };
        }
        public void Add(Author entity)
        {
            entity.ID = Authors.Max(a => a.ID) + 1;
            Authors.Add(entity);
        }

        public void Delete(int id)
        {
            var Author= Find(id);
            Authors.Remove(Author);
        }

        public Author Find(int id)
        {
            var Author = Authors.SingleOrDefault(a => a.ID == id);
            return Author;
        }

        public IList<Author> List()
        {
            return Authors;
        }

        public void Update(int id, Author entity)
        {
            var NewAuthor = Find(id);
            NewAuthor.ID = entity.ID;
            NewAuthor.FullName = entity.FullName;

        }
    }
}
