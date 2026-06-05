public class Asignatura
{
    public int Id {get; set;}
    public required string NombreAsignatura{get; set;}
    public required string Grado{get; set;}

    public int DocenteId{get; set;}
    public required Docente Docente{get; set;}
}