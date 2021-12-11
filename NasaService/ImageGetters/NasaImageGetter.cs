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

        public async Task GetImages(DateOnly imageDate, string directory, CancellationToken stoppingToken)
        {
            string targetParentDir = Path.Combine(directory, imageDate.ToString("yyyy-MM-dd"));

            Logger.LogInformation($"Downloading images from {imageDate:yyyy-MM-dd} to {targetParentDir}.");
            Directory.CreateDirectory(targetParentDir);

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
                string targetPath = Path.Combine(targetParentDir, fileName);

                Logger.LogInformation($"Writing {imageUrl} to {targetPath}.");

                using HttpResponseMessage response = await Client.GetAsync(imageUrl);
                using Stream webStream = await response.Content.ReadAsStreamAsync();
                using FileStream fileStream = new(targetPath, FileMode.Create);

                if (stoppingToken.IsCancellationRequested)
                    return;

                webStream.CopyTo(fileStream);

                Logger.LogInformation($"{fileName} written successfully.");
            }
        }
    }
}