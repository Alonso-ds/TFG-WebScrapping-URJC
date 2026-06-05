using System.ComponentModel.DataAnnotations;

public class Docente
{
    [Key]
    public int Id {get; set;}

    [Required]
    public required string Nombre {get; set;}

    [Required]
    public required string UrlPerfil {get; set;}

    public string? Cargo {get; set;}
    public string? Email {get; set;}
    public string? Centro {get; set;}
    public string? Departamento {get; set;}
    public string? Area {get; set;}
    public string? Biografia {get; set;}

    public int? Quinquenios {get; set;}
    public int? Sexenios {get; set;}
    public int? Docentia {get; set;}

}