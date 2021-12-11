﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NasaService
{
    public class NasaReplyFileReader : INasaReplyReader
    {
        public string FilePath { get; private set; }

        public NasaReplyFileReader(string filePath)
        {
            FilePath = filePath;
        }

        public async Task<string> GetReply(DateOnly date)
        {
            string content = await File.ReadAllTextAsync(FilePath);
            return content;
        }
    }
}
