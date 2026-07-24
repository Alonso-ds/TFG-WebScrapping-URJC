using System.Text.Json.Serialization;
public class TfeDTO
{
    [JsonPropertyName("TIPO")]
    public string? Tipo {get; set;}
    [JsonPropertyName("TITULO")]
    public string? Titulo {get; set;}
    [JsonPropertyName("FECHADEF")]
    public string? FechaDefensa {get; set;}
    [JsonPropertyName("GRADO")]
    public string? Grado {get; set;}
    [JsonPropertyName("TUTOR")]
    public string? Tutor {get; set;}
    [JsonPropertyName("COTUTOR")]
    public string? Cotutor {get; set;}

}