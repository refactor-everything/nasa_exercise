using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NasaService.Models
{
    public class Rover
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("landing_date")]
        public DateTime LandingDate { get; set; }
        
        [JsonPropertyName("launch_date")]
        public DateTime LaunchDate { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
