using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NasaService.Models
{
    public class Photo
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("sol")]
        public int Sol { get; set; }

        [JsonPropertyName("camera")]
        public Camera? Camera { get; set; }

        [JsonPropertyName("img_src")]
        public string? ImgSrc { get; set; }

        [JsonPropertyName("earth_date")]
        public DateTime EarthDate { get; set; }

        [JsonPropertyName("rover")]
        public Rover? Rover { get; set; }
    }
}
