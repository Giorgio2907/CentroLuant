using System.Text.Json;
using System.Text.Json.Serialization;

namespace CentroLuant.Services
{
    public class DniService
    {
        private readonly HttpClient _httpClient;
        private const string Token = "19561|oUM2QvgQJK0VjkraAYdgcmScH5ukMhyyiVenipH7c68087c7";

        public DniService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DniResultado?> ConsultarDni(string dni)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post,
                    "https://apiperu.dev/api/dni");

                request.Headers.Add("Authorization", $"Bearer {Token}");
                request.Headers.Add("Accept", "application/json");

                var body = JsonSerializer.Serialize(new { dni });
                request.Content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Status: {response.StatusCode}");
                Console.WriteLine($"Response: {json}");

                if (!response.IsSuccessStatusCode)
                    return null;

                var apiResponse = JsonSerializer.Deserialize<ApiDniResponse>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return apiResponse?.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
    }

    public class ApiDniResponse
    {
        public bool Success { get; set; }
        public DniResultado? Data { get; set; }
    }

    public class DniResultado
    {
        public string? Numero { get; set; }
        public string? Nombres { get; set; }

        [JsonPropertyName("apellido_paterno")]
        public string? ApellidoPaterno { get; set; }

        [JsonPropertyName("apellido_materno")]
        public string? ApellidoMaterno { get; set; }

        [JsonPropertyName("nombre_completo")]
        public string? NombreCompleto { get; set; }
    }
}