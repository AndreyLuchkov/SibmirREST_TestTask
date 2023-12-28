using SimbirREST_API.Contracts;

namespace SimbirREST_API.Services
{
    public class BlogTypeService(IBlogRepository blogRepository) : IBlogTypeService
    {
        private readonly IBlogRepository _blogRepository = blogRepository;

        public async Task<int?> GetIdByNameAsync(string name)
        {
            return _blogRepository.db.BlogTypes.FirstOrDefault(bt => bt.Name == name)?.Id;
        }
    }
}
