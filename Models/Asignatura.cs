public class Asignatura
{
    public int Id {get; set;}

    public string? Codigo {get; set;}
    public string? Nombre {get; set;}
    public string? TipoEstudio {get; set;}
    public string? Grado {get; set;}
    public string? Horas {get; set;}

    public List<Docente> Docentes {get; set;} = new();

}