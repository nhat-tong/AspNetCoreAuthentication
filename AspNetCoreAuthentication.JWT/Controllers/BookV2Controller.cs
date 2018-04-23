using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreAuthentication.JWT.Data;
using AspNetCoreAuthentication.JWT.Models;
using System.Collections.Generic;
using System.Linq;

namespace AspNetCoreAuthentication.JWT.Controllers
{
    [ApiVersion("2.0")]
    [Produces("application/json")]
    [Route("api/book")]
    [Authorize(Policy = "AgeRestriction")]
    public class BookV2Controller : BookController
    {
        public BookV2Controller(IMapper mapper) : base(mapper)
        {
        }

        [HttpGet("{id:int}")]
        public override IActionResult Get(int id)
        {
            var book = GetBooks().FirstOrDefault(x => x.Id == id);
            if (book == null) return NotFound();

            return Ok(_mapper.Map<Book, BookModel>(book));
        }

        [HttpGet]
        public override IActionResult Get()
        {
            return Ok(_mapper.Map<IEnumerable<Book>, IEnumerable<BookModel>>(GetBooks()));
        }

        private IEnumerable<Book> GetBooks()
        {
            return new List<Book>
            {
                new Book { Id = 5, Author = "Nhat TONG", Title = "book 1" },
                new Book { Id = 6, Author = "Nhat TONG", Title = "book 2" },
                new Book { Id = 7, Author = "Nhat TONG", Title = "book 3" }
            };
        }

        [HttpPost]
        public IActionResult Post([FromBody]BookModel model)
        {
            return Ok("Book has been created successfully!");
        }
    }
}