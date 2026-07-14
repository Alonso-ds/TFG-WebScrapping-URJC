using System.Text.Json.Serialization;
public class RespuestaPublicacionesApi
{
    [JsonPropertyName("publicaciones")]
    public List<PublicacionApiDto> Publicaciones { get; set; }
}

public class PublicacionApiDto
{

    [JsonPropertyName("TITULO")]
    public string? Titulo {get; set;}
    
    [JsonPropertyName("AUTORES")]
    public string? Autores {get; set;}
    
    [JsonPropertyName("FECHA_PUBLICACION")]
    public string? Fecha {get;set; }
    
    [JsonPropertyName("TIPO")]
    public string? Tipo {get;set; }
    
    [JsonPropertyName("DOI")]
    public string? DOI {get; set;}
    
    [JsonPropertyName("SCO_MEJORQ")]
    public string? CuartilScopus {get; set;}

    [JsonPropertyName("WOS_MEJORQ")]
    public string? CuartilWos {get; set;}
}