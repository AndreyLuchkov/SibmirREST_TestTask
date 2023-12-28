using EFDataAccess.Models;
using Mapster;
using SimbirREST_API.Models;

namespace SimbirREST_API.Mapster
{
    public static class MappingConfig
    {
        public static void RegisterMappings(this IServiceCollection services)
        {
            TypeAdapterConfig<Blog, BlogModel>
                .NewConfig()
                .Map(dest => dest.Tags, src => src.Blog_Tags.Select(bt => bt.Tag.Adapt<TagModel>()));

            TypeAdapterConfig<BlogModel, Blog>
                .NewConfig()
                .Ignore(dest => dest.Author, dest => dest.BlogType, dest => dest.Blog_Tags, dest => dest.PublicationDate);
        }
    }
}
