using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace CQRSCoreV2.TestService
{
    public class TestCompressor
    {
        private IContainer container;
        private CompressorService compressorService;

        public TestCompressor(IContainer container)
        {
            this.container = container;

            this.compressorService = container.Resolve<CompressorService>();
        }

        public void Run()
        {
            this.compressorService.CompressReport();
        }
    }
}
