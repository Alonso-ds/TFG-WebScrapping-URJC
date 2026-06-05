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
        var crawler = host.Services.GetRequiredService<Crawler>();
        await crawler.RastreoAsync();
    }
}