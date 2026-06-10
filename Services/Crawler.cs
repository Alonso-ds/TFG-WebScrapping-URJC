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
        for (char letra1 = 'A'; letra1 <= 'Z'; letra1++)
        {
            for (char letra2 = 'A'; letra2 <= 'Z'; letra2++)
            {
                string busqueda = $"{letra1}{letra2}";
                List<DocenteDTO> dtos = await _scrapper.ScrapProfesor(busqueda);
                if(dtos == null ||dtos.Count == 0)
                {
                    Console.WriteLine($"Busqueda {busqueda} sin resultados");
                    continue;
                }
                
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
                    await Task.Delay(1000);
                }
                //int filasInsertadas = await _context.SaveChangesAsync();
                //Console.WriteLine($"{filasInsertadas} profesores con la letra {letra}");
                await Task.Delay(2000);
            }
        }
    }
}