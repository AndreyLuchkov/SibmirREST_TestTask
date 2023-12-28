using EFDataAccess;

namespace SimbirREST_API.Contracts
{
    public interface IBlogRepository
    {
        public BlogContext db { get; }

        public Task<bool> SaveChangesAsync();
    }
}
