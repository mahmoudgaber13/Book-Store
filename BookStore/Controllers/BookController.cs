using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookStore.Models.Repositories;
using BookStore.Models;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.Extensions.Hosting;

namespace BookStore.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookRepository<Book> bookDBRepository;
        private readonly IBookRepository<Author> authorDBRepository;
        private readonly IWebHostEnvironment hosting;
        string FileName = string.Empty;

        // GET: Book

        public BookController(IBookRepository<Book> bookDBRepository, IBookRepository<Author> AuthorDBRepository ,IWebHostEnvironment hosting )
        {
            this.bookDBRepository = bookDBRepository;
            authorDBRepository = AuthorDBRepository;
            this.hosting = hosting;
        }
        public ActionResult Index()
        {
            var books = bookDBRepository.List();

            return View(books);
        }

        public ActionResult Search(string term)
        {
            var Result = bookDBRepository.Search(term);
            return View("Index", Result);
        }

        // GET: Book/Details/5
        public ActionResult Details(int id)
        {
            var book = bookDBRepository.Find(id);

            return View(book);
        }

        // GET: Book/Create
        public ActionResult Create()
        {
            var model = new BookAuthorViewModel
            {
                Authors = FillSelectList()
            };
            return View(model);
        }

        // POST: Book/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookAuthorViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    if(model.File!=null)
                    {
                        string uploads = Path.Combine(hosting.WebRootPath, "Uploads");
                        FileName = (model.File.FileName);
                        string FullPath = Path.Combine(uploads, FileName);
                        model.File.CopyTo(new FileStream(FullPath, FileMode.Create));
                    }
                    if (model.AuthorId == -1)
                    {
                        ViewBag.Message = "Please select Author from list";
                        return View(GetAllAuthors());
                    }
                    var author = authorDBRepository.Find(model.AuthorId);
                    
                    var book = new Book
                    {
                        ID = model.BookId,
                        Title = model.Title,
                        Description = model.description,
                        Author = author,
                        ImgUrl=FileName
                    };
                    bookDBRepository.Add(book);
                    return RedirectToAction(nameof(Index));
                }
                catch(Exception e)
                {
                    return View(e.Message);
                }
            }
            else
            {
                
                ModelState.AddModelError("", "You have to fill all required fields");
                return View(GetAllAuthors());
            }
        }

        // GET: Book/Edit/5
        public ActionResult Edit(int id)
        {
            var book = bookDBRepository.Find(id);
            var AuthorID = book.Author != null ? book.Author.ID : book.Author.ID = 0;

            var ViewModel = new BookAuthorViewModel
            {
                BookId = book.ID,
                Title = book.Title,
                description = book.Description,
                AuthorId = AuthorID,
                Authors = authorDBRepository.List().ToList(),
                imgUrl = book.ImgUrl
            };
            return View(ViewModel);
        }

        // POST: Book/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, BookAuthorViewModel viewModel )
        {
            try
            {
                var author = authorDBRepository.Find(viewModel.AuthorId);
                var Image = viewModel.imgUrl;
                if (viewModel.File != null)
                {
                    string uploads = Path.Combine(hosting.WebRootPath, "Uploads");
                    FileName = viewModel.File.FileName;
                    string FullPath = Path.Combine(uploads, FileName);
                    viewModel.File.CopyTo(new FileStream(FullPath, FileMode.Create));
                    Image = FileName;
                }
                var book = new Book
                {
                    ID = viewModel.BookId,
                    Title = viewModel.Title,
                    Description = viewModel.description,
                    Author = author,
                    ImgUrl = Image
                };
                bookDBRepository.Update(viewModel.BookId, book);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                return View(e.Message);
            }
        }

        // GET: Book/Delete/5
        public ActionResult Delete(int id)
        {
            var book = bookDBRepository.Find(id);

            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int id )
        {
            try
            {
                // TODO: Add delete logic here
                bookDBRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        List<Author> FillSelectList()
        {
            var authors = authorDBRepository.List().ToList();
            authors.Insert(0,new Author {ID=-1,FullName= "Please Select an author.." });
            return authors;
        }
        BookAuthorViewModel GetAllAuthors()
        {
            var model = new BookAuthorViewModel
            {
                Authors = FillSelectList()
            };
            return model;
        }
    }
}