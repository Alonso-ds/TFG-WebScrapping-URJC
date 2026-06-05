using System.ComponentModel.DataAnnotations;

public class Docente
{
    [Key]
    public int Id {get; set;}

    [Required]
    public required string Nombre {get; set;}

    [Required]
    public required string UrlPerfil {get; set;}
}