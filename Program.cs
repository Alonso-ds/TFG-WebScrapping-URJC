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

        var host = builder.Build();
        var crawler = host.Services.GetRequiredService<Crawler>();
        await crawler.RastreoAsync();
    }
}