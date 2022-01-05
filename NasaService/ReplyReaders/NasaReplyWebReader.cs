using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NasaService.Models;

namespace NasaService
{
    public class NasaReplyWebReader : INasaReplyReader
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public string ApiUrl { get; private set; }
        private string ApiKey { get; set; }

        public NasaReplyWebReader(IOptions<NasaWebApiOptions> options,
            IHttpClientFactory httpClientFactory)
        {
            ApiUrl = options.Value.ApiUrl;
            ApiKey = options.Value.ApiKey;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetReply(DateOnly date)
        {
            string dateString = date.ToString("yyyy-MM-dd");

            var client = _httpClientFactory.CreateClient();

            HttpResponseMessage response = await client.GetAsync($"{ApiUrl}?earth_date={dateString}&api_key={ApiKey}");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            return responseBody;
        }
    }
}
