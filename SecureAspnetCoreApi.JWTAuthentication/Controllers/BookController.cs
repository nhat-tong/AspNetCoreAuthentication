using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecureAspnetCoreApi.JWTAuthentication.Data;
using SecureAspnetCoreApi.JWTAuthentication.Models;

namespace SecureAspnetCoreApi.JWTAuthentication.Controllers
{
    [Produces("application/json")]
    [Route("api/[Controller]")]
    [Authorize(Policy = "AgeRestriction")]
    public class BookController : BaseController
    {
        private readonly IMapper _mapper;

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
        public IEnumerable<BookModel> Get()
        {
            var currentUser = HttpContext.User;
            return _mapper.Map<IEnumerable<Book>, IEnumerable<BookModel>>(GetBooks());
        }

        /// <summary>
        /// Get book by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Success</response> 
        [HttpGet("{id:int}", Name = "GetBookById")]
        [ProducesResponseType(200)]
        public BookModel Get(int id)
        {
            return _mapper.Map<Book, BookModel>(GetBooks().FirstOrDefault(x => x.Id == id));
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