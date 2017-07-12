using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSCoreV2
{
    public interface ICompress
    {
        Task<string> DoCompress(string source, string destinationPath);
    }
}
