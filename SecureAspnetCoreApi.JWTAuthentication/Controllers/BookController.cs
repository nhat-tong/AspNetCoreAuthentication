using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureAspnetCoreApi.JWTAuthentication.Data;
using SecureAspnetCoreApi.JWTAuthentication.Models;

namespace SecureAspnetCoreApi.JWTAuthentication.Controllers
{
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/book")]
    [Authorize(Policy = "AgeRestriction")]
    public class BookController : BaseController
    {
        protected readonly IMapper _mapper;

        public BookController(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Return all books
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Sucess. Returns list of books</response>
        /// <response code="400">Error. Bad Request</response>  
        [HttpGet]
        [ProducesResponseType(typeof(Book), 200)]
        [ProducesResponseType(400)]
        public virtual IActionResult Get()
        {
            return Ok(_mapper.Map<IEnumerable<Book>, IEnumerable<BookModel>>(GetBooks()));
        }

        /// <summary>
        /// Get book by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Success</response> 
        [HttpGet("{id:int}", Name = "GetBookById")]
        [ProducesResponseType(200)]
        public virtual IActionResult Get(int id)
        {
            var book = GetBooks().FirstOrDefault(x => x.Id == id);
            if (book == null) return NotFound();

            return Ok(_mapper.Map<Book, BookModel>(book));
        }

        private IEnumerable<Book> GetBooks()
        {
            return new List<Book>
            {
                new Book { Id = 1, Author = "Ray Bradbury",Title = "Fahrenheit 451" },
                new Book { Id = 2, Author = "Gabriel García Márquez", Title = "One Hundred years of Solitude" },
                new Book { Id = 3, Author = "George Orwell", Title = "1984" },
                new Book { Id = 4, Author = "Anais Nin", Title = "Delta of Venus" }
            };
        }
    }
}