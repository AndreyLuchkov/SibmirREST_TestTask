using EFDataAccess.Models;
using SimbirREST_API.Enums;
using SimbirREST_API.Models;

namespace SimbirREST_API.Contracts
{
    public interface IBlogService
    {
        public Task<int?> AddBlogAsync(BlogModel model);

        public Task<BlogModel?> GetBlogAsync(int blogId);

        public Task<IEnumerable<BlogModel>> GetAllBlogsAsync();

        public Task<IEnumerable<BlogModel>> SearchBlogsAsync(int? pageSize, int? pageNum, string? searchText, SortType sort);

        public Task<bool> UpdateBlogAsync(BlogModel model);

        public Task<bool> DeleteBlogAsync(int blogId);
    }
}
