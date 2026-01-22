using Amazon.DynamoDBv2.DataModel;
using Blibliotheque.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blibliotheque.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IDynamoDBContext _dbContext;

        public BooksController(IDynamoDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        static private List<Book> books = new List<Book>
        {
            new Book
            {
                Id = 1,
                Title = "The Great Gatsby",
                Author = "F. Scott Fitzgerald",
                YearPublished = 1925
            },
            new Book
            {
                Id = 2,
                Title = "To Kill a Mockingbird",
                Author = "Harper",
                YearPublished = 1960
            },
            new Book
            {
                Id = 3,
                Title = "1984",
                Author = "George",
                YearPublished = 1940
            },
            new Book
            {
                Id = 4,
                Title = "Pride and Prejudice",
                Author = "Jane Austen",
                YearPublished = 1813
            },
        };

        [HttpGet]
        public async Task<ActionResult<List<Book>>> GetBooks()
        {
            return Ok(await _dbContext.ScanAsync<Book>(new List<ScanCondition>()).GetRemainingAsync());
        }

        [HttpGet("{id}/{author}")]
        public async Task<ActionResult<Book>> GetBookById(int id, string author)
        {
            Book? book = await _dbContext.LoadAsync<Book>(id, author);
            
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [HttpPost]
        public async Task<ActionResult<Book>> AddBook (Book newBook)
        {
            if (newBook == null)
            {
                return BadRequest();
            }

            await _dbContext.SaveAsync(newBook);
            return CreatedAtAction(nameof(GetBookById), new { id = newBook.Id, author = newBook.Author }, newBook);
        }

        [HttpPut("{id}/{author}")]
        public async Task<IActionResult> UpdateBook(int id, string author, Book updatedBook)
        {
            Book? book = await _dbContext.LoadAsync<Book>(id, author);

            if (book == null)
            {
                return NotFound();
            }

            book.Id = updatedBook.Id;
            book.Author = updatedBook.Author;
            book.YearPublished = updatedBook.YearPublished;
            book.Title = updatedBook.Title;

            await _dbContext.SaveAsync(book);
            return NoContent();
        }

        [HttpDelete("{id}/{author}")]
        public async Task<IActionResult> Deletebook(int id, string author)
        {
            Book? book = await _dbContext.LoadAsync<Book>(id, author);

            if (book == null)
            {
                return NotFound();
            }

            await _dbContext.DeleteAsync(book);
            return NoContent();
        }
    }
}
