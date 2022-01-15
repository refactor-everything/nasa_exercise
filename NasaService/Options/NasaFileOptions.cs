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
    public class NasaFileOptions
    {
        /// <summary>
        /// The full path to the file containing a downloaded JSON response.
        /// </summary>
        public string FilePath { get; set; } = string.Empty;
    }
}
