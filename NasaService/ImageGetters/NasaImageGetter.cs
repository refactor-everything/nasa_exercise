using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using NasaService.Models;

namespace NasaService
{
    public class NasaImageGetter : IImageGetter
    {
        private static readonly HttpClient Client = new HttpClient();

        public string ApiUrl { get; set; }
        public string ApiKey { get; set; }

        public NasaImageGetter()
        {

        }

        public async Task<NasaReply?> GetImageUrls(DateOnly date)
        {
            NasaReply? reply = null;
            string dateString = date.ToString("yyyy-MM-dd");

            try
            {
                HttpResponseMessage response = await Client.GetAsync($"{ApiUrl}?earth_date={dateString}&api_key={ApiKey}");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                reply = JsonSerializer.Deserialize<NasaReply>(responseBody);
                return reply;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
