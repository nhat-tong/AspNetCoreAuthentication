using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using SecureAspnetCoreApi.JWTAuthentication.Data;
using SecureAspnetCoreApi.JWTAuthentication.Models;

namespace SecureAspnetCoreApi.JWTAuthentication.AutoMapper
{
    public class BookUrlResolver : IValueResolver<Book, BookModel, string>
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public BookUrlResolver(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string Resolve(Book source, BookModel destination, string destMember, ResolutionContext context)
        {
            // Retrieve UrlHelper from HttpContext.Items of current request
            var urlHelper = (UrlHelper)_contextAccessor.HttpContext.Items["URLHELPER"];
            return urlHelper.Link("GetBookById", new { id = source.Id });
        }
    }
}
