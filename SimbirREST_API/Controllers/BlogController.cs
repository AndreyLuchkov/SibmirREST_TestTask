using EFDataAccess;
using EFDataAccess.Models;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimbirREST_API.Contracts;
using SimbirREST_API.Enums;
using SimbirREST_API.Models;

namespace SimbirREST_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BlogController(IBlogService blogService) : ControllerBase
    {
        private readonly IBlogService _blogService = blogService;

        /// <summary>
        /// Создает блог.
        /// </summary>
        /// <remarks>
        /// {
        ///     "name": "Fugiat occaecat esse deserunt ullamco ex ea et fugiat fugiat.",
        ///     "description": "Adipisicing reprehenderit ex cupidatat id ipsum aliqua in anim esse. Eu excepteur elit ex occaecat cillum sit cupidatat enim ad incididunt. Incididunt sint amet laborum dolore enim labore non. Fugiat cillum mollit proident nisi eiusmod dolor enim id nostrud nostrud sit.\r\n",
        ///     "shortDescription": "Do commodo adipisicing voluptate ea quis ea ut ut aliquip et cupidatat commodo et aliquip.",
        ///     "author": {
        ///         "name": "Guy Sanford"
        ///     },
        ///     "blogType": {
        ///         "name": "архив"
        ///     },
        ///     "tags": [
        ///         {
        ///             "name": "duis"
        ///         },
        ///         {
        ///             "name": "incididunt"
        ///         }
        ///     ]
        /// }
        /// </remarks>
        [HttpPost("add"), Authorize]
        public async Task<IResult> AddBlogAsync(BlogModel model)
        {
            var blog = await _blogService.AddBlogAsync(model);

            return blog != null ? Results.Ok() : Results.BadRequest();
        }

        /// <summary>
        /// Обновляет блог с заданным в JSON полем Id.
        /// </summary>
        /// <returns></returns>
        [HttpPost("update"), Authorize]
        public async Task<IResult> UpdateBlogAsync(BlogModel model)
        {
            bool success = await _blogService.UpdateBlogAsync(model);

            return success ? Results.Ok() : Results.BadRequest();
        }

        /// <summary>
        /// Удаляет блог с заданным id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("delete/{id}"), Authorize]
        public async Task<IResult> DeleteBlogAsync(int id)
        {
            bool success = await _blogService.DeleteBlogAsync(id);

            return success ? Results.Ok() : Results.BadRequest();
        }

        /// <summary>
        /// Возвращает список блогов, или, если определен параметр "id", то блог с заданным id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchText"></param>
        /// <param name="pageNum"></param>
        /// <param name="sortType"></param>
        /// <returns></returns>
        [HttpGet("blogs")]
        public async Task<IResult> GetBlogsAsync([FromQuery] int? id, [FromQuery] int? pageSize, [FromQuery] int? pageNum, [FromQuery] string? searchText, [FromQuery] SortType sortType)
        {
            if (id != null)
            {
                var blog = await _blogService.GetBlogAsync(id.Value);

                return Results.Json(blog);
            }
            else
            {
                var blogs = await _blogService.SearchBlogsAsync(pageSize, pageNum, searchText, sortType);

                return Results.Json(blogs);
            }
        }
    }
}
