public class Publicacion
{
    public int Id { get; set; }
    public string Titulo { get; set; } = null!;
    public string? Autores { get; set; }
    public string? Tipo { get; set; }
    public string? Fecha { get; set; }
    public string? Doi { get; set; }
    public string? Cuartil { get; set; }

    public List<Docente> Docentes { get; set; } = new List<Docente>();
}