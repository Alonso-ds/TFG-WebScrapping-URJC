using System;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

public class Scrapper
{

    private readonly HttpClient _client;

    public Scrapper(HttpClient client)
    {
        _client = client;
    }

    public async Task<List<DocenteDTO>> ScrapProfesor(string letra)
    {
        var listaDocentes = new List<DocenteDTO>();
        var url = "https://servicios.urjc.es/pdi/";
        var formData = new Dictionary<string, string>
        {
             { "busqueda", letra },
             { "busquedaApellido", "1"}
        };

        var content = new FormUrlEncodedContent(formData);
        try
        {
            HttpResponseMessage response = await _client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            //await System.IO.File.WriteAllTextAsync($"debug_letra{letra}.html", responseBody);
            //Console.WriteLine($"[DEBUG] HTML guardado para la letra {letra}");

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(responseBody);

            var nodosProfesores = htmlDoc.DocumentNode.SelectNodes("//a[contains(@href, 'ver/')]");
            if (nodosProfesores != null)
            {
                foreach (var nodo in nodosProfesores)
                {
                    string profesorName = nodo.InnerText.Trim();
                    if (!string.IsNullOrWhiteSpace(profesorName))
                    {
                        var docente = new DocenteDTO{Nombre = profesorName, UrlPerfil = "https://servicios.urjc.es" + nodo.GetAttributeValue("href","")};
                        listaDocentes.Add(docente);
                        Console.WriteLine($"Profesor encontrado: {profesorName}");    
                    }
                }
            }
            else
            {
                Console.WriteLine("No se encontraron profesores con esa letra.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al acceder a la web: {ex.Message}");
        }
        return listaDocentes;
    }
}
