﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSCoreV2
{
    public class CompressorC:ICompress 
    {
        public async Task<string> DoCompress(string source, string destinationPath)
        {
            Console.WriteLine("Compressor C");
            return "C";
        }
    }
}
