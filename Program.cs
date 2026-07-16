using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddHttpClient<Scrapper>();
        builder.Services.AddTransient<Crawler>();

        builder.Services.AddDbContext<UniDbContext>(options => options.UseSqlite("Data Source=universidad.db"));

        var host = builder.Build();
        using (var scope = host.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<UniDbContext>();
            await dbContext.Database.MigrateAsync();
            await dbContext.Database.ExecuteSqlRawAsync("PRAGMA journal_mode=WAL;");
        }
        var crawler = host.Services.GetRequiredService<Crawler>();
        try
        {
            await crawler.RastreoAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Crash message: {ex.Message}");
        }

    }
}