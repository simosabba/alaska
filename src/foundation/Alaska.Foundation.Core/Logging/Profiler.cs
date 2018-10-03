using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaska.Foundation.Core.Logging
{
    public class Profiler : IDisposable
    {
        private LogLevel _level;
        private string _title;
        private Stopwatch _stopwatch = new Stopwatch();

        public Profiler(string title)
            : this(title, LogLevel.Trace)
        { }

        public Profiler(string title, LogLevel level)
        {
            _level = level;
            _title = title;
            _stopwatch.Start();
            Logger.Current.Log(_level, $"START | {title}");
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            Logger.Current.Log(_level, $"COMPLETED | {_title} | Elapsed: {_stopwatch.ElapsedMilliseconds} ms");
        }
    }

    public class InfoProfiler : Profiler
    {
        public InfoProfiler(string title)
            : base(title, LogLevel.Information)
        { }
    }
}
