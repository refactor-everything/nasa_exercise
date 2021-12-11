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
        private static readonly HttpClient Client = new();

        private INasaReplyReader ReplyReader { get; set; }
        private readonly ILogger<Worker> Logger;

        public NasaImageGetter(ILogger<Worker> logger, INasaReplyReader replyReader)
        {
            Logger = logger;
            ReplyReader = replyReader;
        }

        public async Task GetImages(DateOnly imageDate)
        {
            string jsonReply = await ReplyReader.GetReply(imageDate);
            Logger.LogDebug(jsonReply);

            NasaReply? nasaReply = JsonSerializer.Deserialize<NasaReply?>(jsonReply);

            if (nasaReply == null || nasaReply.Photos == null)
                return;

            foreach(Photo photo in nasaReply.Photos)
            {
                if (photo.ImgSrc == null)
                    continue;

                string imageUrl = photo.ImgSrc;
                string fileName = Path.GetFileName(imageUrl);

                using HttpResponseMessage response = await Client.GetAsync(imageUrl);
                using Stream webStream = await response.Content.ReadAsStreamAsync();
                using FileStream fileStream = new(fileName, FileMode.Create);

                Logger.LogInformation($"Writing {imageUrl} to {fileStream.Name}.");

                webStream.CopyTo(fileStream);
            }
        }
    }
}