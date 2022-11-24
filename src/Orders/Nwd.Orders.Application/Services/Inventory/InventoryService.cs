using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Nwd.Orders.Application.Services.Inventory
{
    public class InventoryService : IInventoryService
    {
        public InventoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        private readonly HttpClient _httpClient;



        public async Task UpdateAsync(UpdateInventoryModel decreaseInventoryModel, string accessToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, "api/inventory/decrease")
            {
                Content = JsonContent.Create(decreaseInventoryModel)
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
    }
}