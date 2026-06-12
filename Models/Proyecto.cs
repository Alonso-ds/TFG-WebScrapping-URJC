public class Proyecto
{
    public int Id{get; set;}
    public string Titulo{get; set;} = string.Empty;

    public string? FechaInicio{get; set;}
    public string? FechaFinal{get; set;}
    public string? EntidadFinanciadora{get; set;}
    public string? RefExterna{get; set;}
    public string? RefInterna{get; set;}

    public string? InvPrincipales{get; set;}
    public string? Investigadores{get; set;}
    public string? InvestigadoresTecnicos{get; set;}
    public string? Colaboradores{get; set;}

    public List<Docente> Docentes{get; set;} = new();
}