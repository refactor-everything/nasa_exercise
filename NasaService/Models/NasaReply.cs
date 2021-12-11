using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using NasaService.Models;

namespace NasaService.Models
{
    public class NasaReply
    {
        [JsonPropertyName("photos")]
        public List<Photo>? Photos { get; set; }
    }
}
