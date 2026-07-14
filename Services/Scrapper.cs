using System;
using System.Net.Http;
using System.Net.Mime;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using System.Text.Json;

public class Scrapper
{

    private readonly HttpClient _client;
    private readonly HttpClient _httpClient = new HttpClient();

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

    public async Task ScrapDetallesProfesor(Docente docente, UniDbContext context)
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

            ExtraerProyectos(htmlDoc.DocumentNode, docente, context);

            docente.Quinquenios = ExtraerInt(htmlDoc, "Quinquenios");
            docente.SexeniosInvestigación = ExtraerInt(htmlDoc, "Sexenios investigación");
            docente.SexeniosTransferencia = ExtraerInt(htmlDoc, "Sexenios transferencia");
            docente.Docentia = ExtraerInt(htmlDoc, "Docentia");

            ExtraerPublicaciones(htmlDoc.DocumentNode, docente, context);
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
    
    private void ExtraerProyectos(HtmlNode documentNode, Docente docente, UniDbContext context)
    {
        var nodosProyectos = documentNode.SelectNodes("//div[@id='tab_proyectos']//div[contains(@class, 'panel-default')]");
        if (nodosProyectos == null)
        {
            Console.WriteLine("No se encontraron proyectos");
            return;
        }

        Console.WriteLine($"Se han encontrado {nodosProyectos.Count} proyectos");

        foreach(var nodoProyecto in nodosProyectos)
        {
            var nodoTitulo = nodoProyecto.SelectSingleNode(".//a[contains(@class, 'accordion-toggle')]") ?? nodoProyecto.SelectSingleNode(".//h4[contains(@class, 'panel-title')]");
            if(nodoTitulo == null) continue;

            string tituloProyecto = nodoTitulo.InnerText.Replace("\n", "").Replace("\r", "").Trim();
            if(string.IsNullOrWhiteSpace(tituloProyecto)) continue;
            // Y si mejor por referencia interna?
            Proyecto ? proyectoExistente = context.Proyectos.FirstOrDefault(p => p.Titulo == tituloProyecto);
            if(proyectoExistente != null)
            {
                if (!docente.Proyectos.Contains(proyectoExistente))
                {
                    docente.Proyectos.Add(proyectoExistente);
                    Console.WriteLine("Proyecto actualizado");
                }
                continue;
            }
            var nuevoProyecto = new Proyecto{Titulo = tituloProyecto};

            var textos = nodoProyecto.SelectNodes(".//div[@class='panel-body']//p");

            if(textos != null)
            {
                foreach(var texto in textos)
                {
                    string textoLimpio = texto.InnerText.Replace("&nbsp", " ").Replace("\u00A0", " ").Trim().ToLower();

                    if(textoLimpio.Contains("fecha inicio:") || textoLimpio.Contains("fecha fin:") || textoLimpio.Contains("referencia externa") || textoLimpio.Contains("referencia interna") || textoLimpio.Contains("entidad financiadora"))
                    {
                        Console.WriteLine("INFO METADATO");
                        ExtraerTextoSuelto(texto, nuevoProyecto);
                    }
                    else if(textoLimpio.Contains("principal") || textoLimpio.Contains("investigadores") || textoLimpio.Contains("técnico") || textoLimpio.Contains("colaboradores"))
                    {
                        Console.WriteLine("INFO NOMBRES");
                        ExtraerNombres(texto, nuevoProyecto);
                    }
                    else
                    {
                        Console.WriteLine("TEXTO INFO PROYECTO IGNORADO");
                    }
                }
            }
            docente.Proyectos.Add(nuevoProyecto);
            Console.WriteLine($"Proyecto nuevo {tituloProyecto}");
        }
    }

    private void ExtraerTextoSuelto(HtmlNode texto, Proyecto nuevoProyecto)
    {
        var nodosB = texto.SelectNodes(".//b");
        if (nodosB == null) return;

        foreach(var linea in nodosB)
        {
            string etiqueta = linea.InnerText.Replace("&nbsp", " ").Replace("\u00A0", " ").Replace(":","").Trim().ToLower();
            var NextLine = linea.NextSibling;
            string content = NextLine?.InnerText.Replace("&nbsp", " ").Replace("\u00A0", " ").Replace("\n", "").Replace("\r", "").Trim() ?? "";

            if (string.IsNullOrWhiteSpace(content))
            {
                content = NextLine.NextSibling.InnerText.Replace("&nbsp", " ").Replace("\u00A0", " ").Trim();
            }

            if(etiqueta.Contains("fecha inicio")) nuevoProyecto.FechaInicio = content;
            else if(etiqueta.Contains("fecha fin")) nuevoProyecto.FechaFinal = content;
            else if(etiqueta.Contains("entidad financiadora")) nuevoProyecto.EntidadFinanciadora = content;
            else if(etiqueta.Contains("referencia externa")) nuevoProyecto.RefExterna = content;
            else if(etiqueta.Contains("referencia interna")) nuevoProyecto.RefInterna = content;
        }
    }

    private void ExtraerNombres(HtmlNode texto, Proyecto nuevoProyecto)
    {
        var nodoB = texto.SelectSingleNode(".//b");
        if(nodoB == null) return;

        string etiqueta = nodoB.InnerText.Replace("&nbsp", " ").Replace("\u00A0", " ").Replace(":", "").Trim().ToLower();
        var nodoUL = texto.SelectSingleNode("following-sibling::ul[1]");

        if(nodoUL != null)
        {
            var nodosLI = nodoUL.SelectNodes(".//li");
            if(nodosLI != null)
            {
                var nombres = nodosLI.Select(n => n.InnerText.Replace("\n", "").Trim()).Where(n => !string.IsNullOrWhiteSpace(n));
                string content = string.Join(",", nombres);

                if(etiqueta.Contains("principal")) nuevoProyecto.InvPrincipales = content;
                else if(etiqueta.Contains("técnico") || etiqueta.Contains("tecnico")) nuevoProyecto.InvestigadoresTecnicos = content;
                else if(etiqueta.Contains("colaboradores")) nuevoProyecto.Colaboradores = content;
                else if(etiqueta.Contains("investigadores")) nuevoProyecto.Investigadores = content;
            }
        }
    } 

    private async Task ExtraerPublicaciones(HtmlNode nodo, Docente docente, UniDbContext context)
    {
       var nodoIdPdi = nodo.SelectSingleNode("//input[@name='idPDI']");
       if (nodoIdPdi == null)
        {
            Console.WriteLine("No se encuentra el ID en PDI");
            return;
        }
        string idPDI = nodoIdPdi.GetAttributeValue("value", "").Trim();
        if(string.IsNullOrEmpty(idPDI)) return;

        string urlAPI = $"https://servicios.urjc.es/pdi/public/api/investigadores/{idPDI}/publicaciones";

        try
        {
            string jsonRespuesta = await _httpClient.GetStringAsync(urlAPI);
            var datosApi = JsonSerializer.Deserialize<RespuestaPublicacionesApi>(jsonRespuesta);
            if(datosApi == null || datosApi.Publicaciones.Count == 0) return;

            Console.WriteLine($"Descargadas {datosApi.Publicaciones.Count} publicaciones");

            foreach(var pubDTO in datosApi.Publicaciones)
            {
                if(string.IsNullOrWhiteSpace(pubDTO.Titulo)) continue;
                string titulo = pubDTO.Titulo.Trim();

                Publicacion? pubExistente = context.Publicaciones.FirstOrDefault(p => p.Titulo == titulo);

                if(pubExistente != null)
                {
                    if (!docente.Publicaciones.Contains(pubExistente))
                    {
                        docente.Publicaciones.Add(pubExistente);
                        Console.WriteLine($"Publicacion existente enlazada {titulo}");
                    }
                    continue;
                }
                var nuevaPubli = new Publicacion
                {
                    Titulo = titulo,
                    Autores = pubDTO.Autores,
                    Tipo = pubDTO.Tipo,
                    Fecha = pubDTO.Fecha,
                    Doi = pubDTO.DOI,
                    Cuartil = $"{pubDTO.CuartilScopus} {pubDTO.CuartilWos}".Trim()
                };
                docente.Publicaciones.Add(nuevaPubli);
                Console.WriteLine($"Nueva publicacion! {titulo}");
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error al extraer publicaciones: {ex.Message}");
        }
    }
}
