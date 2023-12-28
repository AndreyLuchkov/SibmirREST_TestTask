using EFDataAccess.Models;
using SimbirREST_API.Models;

namespace SimbirREST_API.Contracts
{
    public interface ITagService
    {
        public Task<Tag?> GetTagByNameAsync(string name);

        public Task<int?> AddTagAsync(TagModel model);
    }
}
