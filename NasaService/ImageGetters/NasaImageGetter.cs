using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NasaService.Models;

namespace NasaService
{
    /// <summary>
    /// Class that fetches images from NASA.gov.
    /// </summary>
    public class NasaImageGetter : IImageGetter
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private INasaReplyReader _replyReader { get; set; }
        private readonly ILogger<Worker> _logger;

        public NasaImageGetter(ILogger<Worker> logger,
            INasaReplyReader replyReader,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _replyReader = replyReader;
            _httpClientFactory = httpClientFactory;
        }

        public async Task GetImages(DateOnly imageDate, string directory, CancellationToken stoppingToken)
        {
            // Save the files to a path that includes the date.
            string targetDir = Path.Combine(directory, imageDate.ToString("yyyy-MM-dd"));

            if(!Path.IsPathFullyQualified(targetDir))
            {
                targetDir = Path.Combine(Environment.CurrentDirectory, targetDir);
            }

            if(!Directory.Exists(targetDir))
                Directory.CreateDirectory(targetDir);

            _logger.LogInformation($"Downloading images from {imageDate:yyyy-MM-dd} to {targetDir}.");

            // Get the reply (either from the NASA website or the downloaded json file -- whichever is configured)
            string jsonReply = await _replyReader.GetReply(imageDate);
            _logger.LogDebug(jsonReply);

            // Convert json to object.
            NasaReply? nasaReply = JsonSerializer.Deserialize<NasaReply?>(jsonReply);

            // Return early if the reply was not deserializable, or if there are no photos.
            if (nasaReply == null || nasaReply.Photos == null)
                return;

            // Create HTTP client.
            var client = _httpClientFactory.CreateClient();

            // Get each photo.
            foreach (Photo photo in nasaReply.Photos)
            {
                if (photo.ImgSrc == null)
                    continue;

                // The API is returing queries with "jpl" in the host name, which is causing "301 Permanently Moved" messages.
                // If I leave the URL intact, the initial GET request will work after 30-45 seconds. Subsequent image GET
                // requests will go through instantaneously.
                // I put this here because the initial wait was bothering me. This is hacky, but it works (for now).
                string imageUrl = Regex.Replace(photo.ImgSrc, @"^https?://mars.jpl.nasa.gov", "https://mars.nasa.gov");

                string fileName = Path.GetFileName(imageUrl);
                string targetPath = Path.Combine(targetDir, fileName);

                _logger.LogInformation($"Writing {imageUrl} to {targetPath}.");

                using Stream webStream = await client.GetStreamAsync(imageUrl, stoppingToken);
                using FileStream fileStream = new(targetPath, FileMode.Create);

                await webStream.CopyToAsync(fileStream, stoppingToken);

                _logger.LogInformation($"{fileName} written successfully.");
            }
        }
    }
}