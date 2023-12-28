using EFDataAccess.Models;
using Mapster;
using SimbirREST_API.Contracts;
using SimbirREST_API.Models;
using SimbirREST_API.Repository;

namespace SimbirREST_API.Services
{
    public class TagService(IBlogRepository blogRepository) : ITagService
    {
        private readonly IBlogRepository _blogRepository = blogRepository;

        public async Task<int?> AddTagAsync(TagModel model)
        {
            var tag = await this.GetTagByNameAsync(model.Name);

            if (tag != null)
            {
                return tag.Id;
            }

            tag ??= model.Adapt<Tag>();

            _blogRepository.db.Tags.Add(tag);

            return await _blogRepository.SaveChangesAsync() ? tag.Id : null;
        }

        public async Task<Tag?> GetTagByNameAsync(string name)
        {
            return _blogRepository.db.Tags.FirstOrDefault(t => t.Name == name);
        }
    }
}
