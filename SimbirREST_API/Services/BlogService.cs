using EFDataAccess.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SimbirREST_API.Contracts;
using SimbirREST_API.Enums;
using SimbirREST_API.Models;

namespace SimbirREST_API.Services
{
    public class BlogService(IBlogRepository blogRepository, ITagService tagService, IAuthorService authorService, IBlogTypeService blogTypeService) : IBlogService
    {
        private readonly IBlogRepository _blogRepository = blogRepository;
        private readonly ITagService _tagService = tagService;
        private readonly IAuthorService _authorService = authorService;
        private readonly IBlogTypeService _blogTypeService = blogTypeService;

        public async Task<int?> AddBlogAsync(BlogModel model)
        {
            var blog = model.Adapt<Blog>();

            foreach (var tagModel in model.Tags)
            {
                var tagId = await _tagService.AddTagAsync(tagModel);

                if (tagId != null)
                {
                    blog.Blog_Tags.Add(new Blog_Tag
                    {
                        TagId = tagId.Value,
                        Blog = blog
                    });
                }
            }

            blog.AuthorId = await _authorService.AddAuthorAsync(model.Author) ?? 0;
            blog.BlogTypeId = await _blogTypeService.GetIdByNameAsync(model.BlogType.Name) ?? 0;

            if (model.BlogType.Name == "архив")
            {
                blog.PublicationDate = DateTime.Today;
            }

            blog.CreatedOn = DateTime.Today;
            blog.ModifiedOn = DateTime.Today;

            _blogRepository.db.Blogs.Add(blog);

            return await _blogRepository.SaveChangesAsync() ? blog.Id : null;
        }

        public async Task<bool> DeleteBlogAsync(int blogId)
        {
            var blog = _blogRepository.db.Blogs.FirstOrDefault(b => b.Id == blogId);
            if (blog == null)
            {
                return false;
            }

            _blogRepository.db.Blogs.Remove(blog);

            return await _blogRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<BlogModel>> GetAllBlogsAsync()
        {
            return _blogRepository.db.Blogs
                .Include(b => b.Author)
                .Include(b => b.BlogType)
                .Include(b => b.Blog_Tags)
                    .ThenInclude(bt => bt.Tag)
                .ProjectToType<BlogModel>()
                .ToList();
        }

        public async Task<BlogModel?> GetBlogAsync(int blogId)
        {
            var blog = _blogRepository.db.Blogs
                .Include(b => b.Author)
                .Include(b => b.BlogType)
                .Include(b => b.Blog_Tags)
                    .ThenInclude(bt => bt.Tag)
                .FirstOrDefault(b => b.Id == blogId);

            return blog.Adapt<BlogModel>();
        }

        public async Task<IEnumerable<BlogModel>> SearchBlogsAsync(int? pageSize, int? pageNum, string? searchQuery, SortType sortType)
        {
            var blogs = _blogRepository.db.Blogs.AsQueryable();
            if (searchQuery != null)
            {
                blogs = blogs.Where(i 
                    => i.Name.Contains(searchQuery)
                    || i.Author.Name.Contains(searchQuery));
            }

            blogs = sortType switch
            {
                SortType.Asc => blogs.OrderBy(i => i.PublicationDate),
                SortType.Desc => blogs.OrderByDescending(i => i.PublicationDate),
                _ => blogs,
            };

            if (pageNum != null)
            {
                pageSize ??= Constants.DefaultPageSize;

                int startAt = pageSize.Value * (pageNum.Value - 1);
                blogs = blogs.Skip(startAt).Take(pageSize.Value);
            }

            return blogs
                .Include(b => b.Author)
                .Include(b => b.BlogType)
                .Include(b => b.Blog_Tags)
                    .ThenInclude(bt => bt.Tag)
                .ProjectToType<BlogModel>()
                .ToList();
        }

        public async Task<bool> UpdateBlogAsync(BlogModel model)
        {
            var blog = _blogRepository.db.Blogs
                .Include(b => b.BlogType)
                .Include(b => b.Blog_Tags)
                .FirstOrDefault(b => b.Id == model.Id);
            if (blog == null)
            {
                return false;
            }

            blog.Blog_Tags = [];
            foreach (var tagModel in model.Tags)
            {
                var tagId = await _tagService.AddTagAsync(tagModel);

                if (tagId != null)
                {
                    blog.Blog_Tags.Add(new Blog_Tag
                    {
                        TagId = tagId.Value,
                        Blog = blog
                    });
                }
            }

            if (model.BlogType.Name != blog.BlogType.Name && model.BlogType.Name == "архив")
            {
                blog.PublicationDate = DateTime.Today;
                blog.BlogTypeId = await _blogTypeService.GetIdByNameAsync(model.BlogType.Name) ?? blog.BlogTypeId;
            }

            blog.AuthorId = await _authorService.AddAuthorAsync(model.Author) ?? blog.AuthorId;

            blog.ModifiedOn = DateTime.Today;

            return await _blogRepository.SaveChangesAsync();
        }
    }
}
