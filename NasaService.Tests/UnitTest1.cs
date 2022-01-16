using Microsoft.Extensions.Options;
using NasaService.Models;
using System;
using System.Text.Json;
using Xunit;

namespace NasaService.Tests
{
    public class UnitTest1
    {
        private NasaFileOptions FileOptions { get; set; }
        private IOptions<NasaFileOptions> NasaFileOptions { get; set; }

        public UnitTest1()
        {
            FileOptions = new()
            {
                FilePath = "example_response.json"
            };

            NasaFileOptions = Options.Create(FileOptions);
        }

        [Fact]
        public async void CanReadJsonResponse()
        {
            NasaReplyFileReader fileReader = new(NasaFileOptions);
            string reply = await fileReader.GetReply(new DateOnly(2022, 1, 1));

            Assert.NotEmpty(reply);
        }

        [Fact]
        public async void CanDeserializeJsonResponse()
        {
            NasaReplyFileReader fileReader = new(NasaFileOptions);
            string reply = await fileReader.GetReply(new DateOnly(2022, 1, 1));

            // Convert json to object.
            NasaReply? nasaReply = JsonSerializer.Deserialize<NasaReply?>(reply);

            Assert.NotNull(nasaReply);

            if(nasaReply != null)
                Assert.NotNull(nasaReply.Photos);
        }
    }
}