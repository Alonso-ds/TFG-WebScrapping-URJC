public class Tfe
{
    public int Id{get; set;}
    public string? Tipo{get; set;}
    public string? Titulo {get; set;}
    public string? FechaDefensa{get; set;}
    public string? Grado{get; set;}
    public string? Tutor{get; set;}
    public string? Cotutor{get; set;}

    public List<Docente> Docentes {get; set;} = new();
}