using System;
using System.Threading.Tasks;


public class Crawler
{
    private readonly Scrapper _scrapper;

    public Crawler(Scrapper scrapper)
    {
        _scrapper = scrapper;
    }

    public async Task RastreoAsync()
    {
        for (char letra = 'A'; letra <= 'Z'; letra++)
        {
            Console.WriteLine($"Letra: {letra}");
            await _scrapper.ScrapProfesor(letra.ToString());
            await Task.Delay(new Random().Next(2000, 4000));
        }
    }
}