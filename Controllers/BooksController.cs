using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleBookAPI.Data;
using SimpleBookAPI.DTOs.BookDTOs;
using SimpleBookAPI.Models;

namespace SimpleBookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly DataContext _context;

        public BooksController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("GetBooks")]
        public async Task<ActionResult<IEnumerable<GetBooksDTO>>> GetBooks()
        {
            var books = from book in _context.Books
                        join author in _context.Authors
                        on book.AuthorId equals author.Id
                        select new GetBooksDTO
                        {
                            Id = book.Id,
                            Title = book.Title,
                            AuthorName = author.FirstName + " " + author.LastName,
                        };
            if (books.Count() > 0)
                return Ok(await books.ToListAsync());
            return NotFound("No record found");
        }

        [HttpGet("GetBookDetailById/{id}")]
        public ActionResult<GetBookDetailDTO> GetBookDetailById(int id)
        {
            var bookDetail = from book in _context.Books
                             join category in _context.Categories
                             on book.CategoryId equals category.Id
                             join publisher in _context.Publishers
                             on book.PublisherId equals publisher.Id
                             join author in _context.Authors
                             on book.AuthorId equals author.Id
                             where book.Id == id
                             select new GetBookDetailDTO
                             {
                                 Id = book.Id,
                                 Title = book.Title,
                                 AuthorName = author.FirstName + " " + author.LastName,
                                 CategoryName = category.Name,
                                 Language = book.Language,
                                 PageCount = book.PageCount,
                                 PublisherName = publisher.Name,
                             };
            if (bookDetail.Count() > 0)
                return Ok(bookDetail);
            return NotFound("No record found");
        }

        [HttpPut("UpdateBook{id}")]
        public async Task<ActionResult<CreateOrUpdateBookDTO>> UpdateBook(int id, CreateOrUpdateBookDTO book)
        {
            var resBook = _context.Books.SingleOrDefault(x => x.Id == id);
            if (resBook == null)
                return NotFound("No record found");

            resBook.Title = book.Title;
            resBook.Language = book.Langueage;
            resBook.AuthorId = book.AuthorId;
            resBook.CategoryId = book.CategoryId;
            resBook.PageCount = book.PageCount;
            resBook.PublisherId = book.PublisherId;

            await _context.SaveChangesAsync();
            return Ok(book);
        }

        [HttpPost("CreateBook")]
        public async Task<ActionResult<CreateOrUpdateBookDTO>> CreateBook(CreateOrUpdateBookDTO book)
        {
            try
            {
                Book newBook = new()
                {
                    Title = book.Title,
                    Language = book.Langueage,
                    AuthorId = book.AuthorId,
                    PageCount = book.PageCount,
                    CategoryId = book.CategoryId,
                    PublisherId = book.PublisherId
                };

                await _context.Books.AddAsync(newBook);
                await _context.SaveChangesAsync();

                return Ok(book);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("DeleteBook/{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound("No record found");
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
