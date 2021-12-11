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
        public Task GetImages(DateOnly imageDate, string directory);
    }
}
