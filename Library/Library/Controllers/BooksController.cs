using Library.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Library.Data;
using Microsoft.AspNetCore.Http;
using Library.Dto;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using System.Collections.Generic;
using System.Diagnostics;

namespace Library.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public BooksController(ApplicationDbContext context, IMapper mapper)
        {
            _db = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> IndexAsync()
        {
            SetSessionUser();

            var loggedUserId = HttpContext.Session.GetString("userId");
            var userBooks = await _db.Books.Where(x => x.UserId == loggedUserId).OrderBy(b => b.Title).ToListAsync();
            var bookDtos = _mapper.Map<IList<BookDto>>(userBooks);

            return View("Index", bookDtos);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBook(BookDto bookDto)
        {
            if (ModelState.IsValid)
            {
                var loggedUserId = HttpContext.Session.GetString("userId");
                
                var newBook = _mapper.Map<Book>(bookDto);
                newBook.UserId = loggedUserId;

                _db.Books.Add(newBook);
                await _db.SaveChangesAsync();

                TempData["message"] = "Dodano nową książkę: " + newBook.Title;
                return RedirectToAction("Index");
            }

            return View();
        }

        public async Task<IActionResult> Delete(int id)
        {
            var book =  await _db.Books.FirstOrDefaultAsync(b => b.Id == id);

            _db.Books.Remove(book);
            await _db.SaveChangesAsync();

            TempData["message"] = "Usunięto książkę: " + book.Title;
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var book = await _db.Books.FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return RedirectToAction("Error404");
            }

            var bookDto = _mapper.Map<BookDto>(book);

            return View(bookDto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var book = await _db.Books.FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return RedirectToAction("Error404");
            }

            var bookToEdit = _mapper.Map<BookDto>(book);

            return View(bookToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> EditBook(BookDto bookDto)
        {
            var book = await _db.Books.FirstOrDefaultAsync(b => b.Id == bookDto.Id);

            _mapper.Map(bookDto, book);

            await _db.SaveChangesAsync();

            TempData["message"] = "Pomyślnie zaktualizowano książkę";
            return RedirectToAction("Index");
        }

        private void SetSessionUser()
        {
            var email = ((ClaimsIdentity)HttpContext.User.Identity).FindFirst(ClaimTypes.Email).Value;
            var user = _db.Users.FirstOrDefault(x => x.Email == email);
            HttpContext.Session.SetString("userId", user.Id);
        }
        
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        public IActionResult Error404()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
