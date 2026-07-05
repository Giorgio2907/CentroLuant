using System.Text.Json;

namespace CentroLuant.Services
{
    public class TipoCambioService
    {
        private readonly HttpClient _httpClient;

        public TipoCambioService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> ObtenerTipoCambio()
        {
            try
            {
                var response = await _httpClient.GetAsync(
                    "https://api.exchangerate-api.com/v4/latest/PEN");

                if (!response.IsSuccessStatusCode)
                    return 3.70m;

                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<ExchangeResponse>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (data?.Rates != null && data.Rates.ContainsKey("USD"))
                    return Math.Round((decimal)data.Rates["USD"], 4);

                return 3.70m;
            }
            catch
            {
                return 3.70m;
            }
        }
    }

    public class ExchangeResponse
    {
        public Dictionary<string, double>? Rates { get; set; }
    }
}