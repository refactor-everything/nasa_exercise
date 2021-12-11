using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NasaService
{
    public interface INasaReplyReader
    {
        public Task<string> GetReply(DateOnly date);
    }
}
