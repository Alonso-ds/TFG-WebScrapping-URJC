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
    public bool TieneBiografia {get; set;}

    public string? GrupoInvestigación {get; set;}
    public string? GrupoDocente {get; set;}
    public string? CentroInvestigación {get; set;}

    public int? Quinquenios {get; set;}
    public int? SexeniosInvestigación {get; set;}

    public int? SexeniosTransferencia {get; set;}
    public int? Docentia {get; set;}

}