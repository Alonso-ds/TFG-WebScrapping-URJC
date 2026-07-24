using System.Text.Json.Serialization;
public class AsignaturaDTO
{
    [JsonPropertyName("TIPOESTUDIO")]
    public string? TipoEstudio {get; set;}
    [JsonPropertyName("CODIGO")]
    public string? Codigo {get; set;}
    [JsonPropertyName("NOMBRE")]
    public string? Nombre {get; set;}
    [JsonPropertyName("GRADO")]
    public string? Grado {get; set;}
    [JsonPropertyName("HORAS")]
    public string? Horas {get; set;}

}