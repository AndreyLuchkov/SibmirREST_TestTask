using EFDataAccess.Models;
using SimbirREST_API.Models;

namespace SimbirREST_API.Contracts
{
    public interface IAuthorService
    {
        public Task<int?> AddAuthorAsync(AuthorModel model);
        public Task<Author> GetAuthorByName(string name);
    }
}
