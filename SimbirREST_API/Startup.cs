using EFDataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SimbirREST_API.Contracts;
using SimbirREST_API.Mapster;
using SimbirREST_API.Repository;
using SimbirREST_API.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;

namespace SimbirREST_API;

public class Startup
{   
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var configJwtOptions = Configuration.GetSection("JWTOptions");

                var key = Encoding.UTF8.GetBytes(configJwtOptions["key"]!);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configJwtOptions["Issuer"],
                    ValidateAudience = true,
                    ValidAudience= configJwtOptions["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    []
                }
            });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        services.AddControllers();

        services.AddDbContext<BlogContext>(options =>
        {
            options.UseSqlServer(Configuration.GetConnectionString("MSSQLServer"));
        });

        services.RegisterMappings();

        services.AddScoped<IBlogRepository, BlogRepository>();

        services.AddScoped<ITagService, TagService>();
        services.AddScoped<IBlogService, BlogService>();
        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<IBlogTypeService, BlogTypeService>();

        services.AddScoped<JwtSecurityTokenHandler>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseEndpoints(endpoint =>
        {
            endpoint.MapControllers();
        });
    }
}
