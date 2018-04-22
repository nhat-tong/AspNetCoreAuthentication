using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SecureAspnetCoreApi.JWTAuthentication.Controllers
{
    [Produces("application/json")]
    [Route("api/[Controller]")]
    [Authorize(Policy = "AgeRestriction")]
    public class BookController : Controller
    {
        /// <summary>
        /// Return all books
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Sucess. Returns list of books</response>
        /// <response code="400">Error. Bad Request</response>  
        [HttpGet]
        [ProducesResponseType(typeof(Book), 200)]
        [ProducesResponseType(400)]
        public IEnumerable<Book> Get()
        {
            var currentUser = HttpContext.User;
            var resultBookList = new Book[]
      {
        new Book { Author = "Ray Bradbury",Title = "Fahrenheit 451" },
        new Book { Author = "Gabriel García Márquez", Title = "One Hundred years of Solitude" },
        new Book { Author = "George Orwell", Title = "1984" },
        new Book { Author = "Anais Nin", Title = "Delta of Venus" }
      };

            return resultBookList;
        }

        public class Book
        {
            public string Author { get; set; }
            public string Title { get; set; }
        }
    }
}