using System;
using System.Threading.Tasks;


public class Crawler
{
    private readonly Scrapper _scrapper;
    private readonly UniDbContext _context;

    public Crawler(Scrapper scrapper, UniDbContext context)
    {
        _scrapper = scrapper;
        _context = context;
    }

    public async Task RastreoAsync()
    {
        //await _context.Database.EnsureCreatedAsync();

        for (char letra = 'A'; letra <= 'Z'; letra++)
        {
            Console.WriteLine($"Letra: {letra}");
            List<DocenteDTO> dtos = await _scrapper.ScrapProfesor(letra.ToString());
            foreach (var dto in dtos)
            if (!string.IsNullOrWhiteSpace(dto.Nombre))
            {
                var nuevoDocente = new Docente{Nombre = dto.Nombre, UrlPerfil = dto.UrlPerfil};
                _context.Add(nuevoDocente);
            }
            int filasInsertadas = await _context.SaveChangesAsync();
            Console.WriteLine($"{filasInsertadas} profesores con la letra {letra}");
            await Task.Delay(new Random().Next(2000, 4000));
        }
    }
}