using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NasaService.Models;

namespace NasaService
{
    public interface IImageGetter
    {
        public string ApiUrl { get; set; }
        public string ApiKey { get; set; }

        public Task<NasaReply?> GetImageUrls(DateOnly date);
    }
}
