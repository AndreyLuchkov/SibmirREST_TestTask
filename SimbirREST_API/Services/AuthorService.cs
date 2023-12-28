using EFDataAccess.Models;
using Mapster;
using SimbirREST_API.Contracts;
using SimbirREST_API.Models;

namespace SimbirREST_API.Services
{
    public class AuthorService(IBlogRepository blogRepository) : IAuthorService
    {
        private readonly IBlogRepository _blogRepository = blogRepository;

        public async Task<int?> AddAuthorAsync(AuthorModel model)
        {
            var author = await this.GetAuthorByName(model.Name);

            if (author != null)
            {
                return author.Id;
            }

            author ??= model.Adapt<Author>();

            _blogRepository.db.Authors.Add(author);

            return await _blogRepository.SaveChangesAsync() ? author.Id : null;
        }

        public async Task<Author?> GetAuthorByName(string name)
        {
            return _blogRepository.db.Authors.FirstOrDefault(a => a.Name == name);
        }
    }
}
