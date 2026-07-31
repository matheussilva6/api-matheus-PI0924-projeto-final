using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;

namespace ApiMatheusProjetoFinal.Services
{
    public class ImposterService
    {
        private readonly HttpClient _httpClient;
        private readonly IDistributedCache _cache;

        public ImposterService(HttpClient httpClient, IDistributedCache cache)
        {
            _httpClient = httpClient;
            _cache = cache;
        }

        public async Task<string> GetInventoryAsync(string sku)
        {
            string cacheKey = $"inventory_{sku}";

            var cachedResult = await _cache.GetStringAsync(cacheKey);
            if (cachedResult != null)
            {
                return cachedResult;
            }

            var response = await _httpClient.GetAsync($"/inventory/{sku}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            await _cache.SetStringAsync(cacheKey, content, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            });

            return content;
        }

        public async Task<string> CreatePaymentAsync(object paymentData)
        {
            var json = JsonSerializer.Serialize(paymentData);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/payments", httpContent);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}