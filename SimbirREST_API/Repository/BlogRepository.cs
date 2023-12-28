using EFDataAccess;
using SimbirREST_API.Contracts;

namespace SimbirREST_API.Repository
{
    public class BlogRepository(BlogContext blogContext) : IBlogRepository
    {
        private readonly BlogContext _blogContext = blogContext;

        public BlogContext db => _blogContext;

        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                await _blogContext.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
