using System;
using System.Collections.Generic;
using System.Text;

namespace Serilog
{
    public static class SerilogEnrichExtensions
    {
        public static LoggerConfiguration EnrichWithCommonData(this LoggerConfiguration loggerConfiguration)
        {
            return
                loggerConfiguration
                    .Enrich.FromLogContext()
                    .Enrich.WithMachineName();
        }
    }
}
