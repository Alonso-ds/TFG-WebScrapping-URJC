using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


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
            {
                if (!string.IsNullOrWhiteSpace(dto.Nombre))
                {
                    var docenteExistente = await _context.Docentes.FirstOrDefaultAsync(d => d.UrlPerfil == dto.UrlPerfil);

                    if (docenteExistente != null)
                    {
                        docenteExistente.Nombre = dto.Nombre;
                        await _scrapper.ScrapDetallesProfesor(docenteExistente);
                        Console.WriteLine($"Profesor actualizado: {docenteExistente.Nombre}");
                    }
                    else
                    {
                        var nuevoDocente = new Docente { Nombre = dto.Nombre, UrlPerfil = dto.UrlPerfil };
                        await _scrapper.ScrapDetallesProfesor(nuevoDocente);
                        _context.Add(nuevoDocente);
                        Console.WriteLine($"Profesor nuevo: {nuevoDocente.Nombre}");
                    }
                }
                await _context.SaveChangesAsync();
                await Task.Delay(2000);
            }
            //int filasInsertadas = await _context.SaveChangesAsync();
            //Console.WriteLine($"{filasInsertadas} profesores con la letra {letra}");
            await Task.Delay(2000);
        }
    }
}