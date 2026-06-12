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
                        var docente = new DocenteDTO { Nombre = profesorName, UrlPerfil = "https://servicios.urjc.es" + nodo.GetAttributeValue("href", "") };
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

    public async Task ScrapDetallesProfesor(Docente docente)
    {
        try
        {
            Console.WriteLine($"Perfil de {docente.Nombre}...");
            HttpResponseMessage response = await _client.GetAsync(docente.UrlPerfil);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(responseBody);

            var nodoCargo = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class, 'profile-usertitle-job')]");
            if (nodoCargo != null)
            {
                docente.Cargo = nodoCargo.InnerText.Trim();
                Console.WriteLine($"Cargo: {docente.Cargo}");
            }

            var nodoEmail = htmlDoc.DocumentNode.SelectSingleNode("//a[contains(@href, 'mailto:')]");
            if (nodoEmail != null)
            {
                docente.Email = nodoEmail.InnerText.Trim();
                Console.WriteLine($"Email: {docente.Email}");
            }

            var nodoCentro = htmlDoc.DocumentNode.SelectSingleNode("//h4[contains(text(), 'Centro')]/following-sibling::span");
            if (nodoCentro != null)
            {
                docente.Centro = nodoCentro.InnerText.Trim();
                Console.WriteLine($"Centro: {docente.Centro}");
            }
 
            var nodoDepartamento = htmlDoc.DocumentNode.SelectSingleNode("//h4[contains(text(), 'Departamento')]/following-sibling::a");
            if(nodoDepartamento != null)
            {
                docente.Departamento = nodoDepartamento.InnerText.Trim();
                Console.WriteLine($"Departamento: {docente.Departamento}");
            }
            var nodoArea = htmlDoc.DocumentNode.SelectSingleNode("//h4[contains(text(), 'Área')]/following-sibling::span");
            if(nodoArea != null)
            {
                docente.Area = nodoArea.InnerText.Trim();
                Console.WriteLine($"Área: {docente.Area}");
            }
            var nodoGrupoInv = htmlDoc.DocumentNode.SelectSingleNode("//h4[contains(text(), 'Grupo de investigación')]/following-sibling::a");
            if(nodoGrupoInv!= null)
            {
                docente.GrupoInvestigación = nodoGrupoInv.InnerText.Trim();
                Console.WriteLine($"Grupo investigacion: {docente.GrupoInvestigación}");
            }
            
            var nodoCentroInv = htmlDoc.DocumentNode.SelectSingleNode("//h4[contains(text(), 'Centro/Instituto de investigación')]/following-sibling::a");
            if(nodoCentroInv != null)
            {
                docente.CentroInvestigación = nodoCentroInv.InnerText.Trim();
                Console.WriteLine($"Centro Investigacion: {docente.CentroInvestigación}");
            }
            var nodoGrupoDocente = htmlDoc.DocumentNode.SelectSingleNode("//h4[contains(text(), 'Grupo docente')]/following-sibling::a");
            if(nodoGrupoDocente != null)
            {
                docente.GrupoDocente = nodoGrupoDocente.InnerText.Trim();
                Console.WriteLine($"Grupo Docente: {docente.GrupoDocente}");
            }

            var nodoBiografia = htmlDoc.DocumentNode.SelectSingleNode("//div[@id='tab_presentacion']//li[contains(@class, 'list-group-item')]");
            if(nodoBiografia != null)
            {
                string textoLimpio = nodoBiografia.InnerText.Trim();

                bool esAviso = textoLimpio.Contains("No existe información");

                docente.TieneBiografia = !string.IsNullOrWhiteSpace(textoLimpio) && textoLimpio.Length > 20 && !esAviso; //algo mas de 20 caracteres...
            }
            else
            {
                docente.TieneBiografia = false;
            }
            Console.WriteLine($"¿Biografía?: {(docente.TieneBiografia ? "SÍ" : "NO")}");

            docente.Quinquenios = ExtraerInt(htmlDoc, "Quinquenios");
            docente.SexeniosInvestigación = ExtraerInt(htmlDoc, "Sexenios investigación");
            docente.SexeniosTransferencia = ExtraerInt(htmlDoc, "Sexenios transferencia");
            docente.Docentia = ExtraerInt(htmlDoc, "Docentia");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al acceder al perfil de {docente.Nombre}: {ex.Message}");
        }
    }

    private int? ExtraerInt(HtmlDocument htmlDoc, string tipo)
    {
        string xpath = $"//div[contains(@class, 'profile-stat-text') and contains(., '{tipo}')]/preceding-sibling::div[contains(@class, 'profile-stat-title')]";
        var nodoInt = htmlDoc.DocumentNode.SelectSingleNode(xpath);
        if(nodoInt != null)
        {
            string intLimpio = nodoInt.InnerText.Trim();
            if(int.TryParse(intLimpio, out int resultado))
            {
                return resultado;
            }
        }
        return null;
    } 
}
