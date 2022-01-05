using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NasaService
{
    /// <summary>
    /// Stores options related to JSON files downloaded from NASA.gov.
    /// </summary>
    public class NasaWebApiOptions
    {
        /// <summary>
        /// The NASA API URL.
        /// </summary>
        public string ApiUrl { get; set; } = String.Empty;

        /// <summary>
        /// The NASA API key.
        /// </summary>
        public string ApiKey { get; set; } = String.Empty;
    }
}
