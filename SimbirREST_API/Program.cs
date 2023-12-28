using EFDataAccess;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.CommandLineUtils;
using SimbirREST_API;

public class Program
{
    public static void Main(string[] args)
    {
        var commandLineApplication = new CommandLineApplication(false);
        var doMigrate = commandLineApplication.Option(
            "--ef-migrate",
            "Apply entity framework migrations and exit",
            CommandOptionType.NoValue);
        commandLineApplication.OnExecute(() =>
        {
            ExecuteApp(args, doMigrate);
            return 0;
        });
        commandLineApplication.Execute(args);
    }

    private static void ExecuteApp(string[] args, CommandOption doMigrate)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(builder =>
            {
                builder.UseStartup<Startup>();
            })
            .Build();

        if (doMigrate.HasValue())
        {
            Console.WriteLine("Applyting Entity Framework migrations");
            using var serviceScope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<BlogContext>();
            
            context.Database.Migrate();
            Console.WriteLine("All done");
        }

        host.Run();
    }
}