using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSCoreV2
{
    public interface ICompressorService
    {
        void CompressReport();

    }
    public class CompressorService:ICompressorService
    {
        private IEnumerable<ICompress> compress;

        public CompressorService(IEnumerable<ICompress> compress)
        {
            this.compress = compress;

        }
        public void CompressReport()
        {
            foreach (var compressItem in this.compress)
            {
                var result = compressItem.DoCompress(string.Empty, string.Empty).Result;
                Console.WriteLine(result);
            }
            
        }
    }
}
