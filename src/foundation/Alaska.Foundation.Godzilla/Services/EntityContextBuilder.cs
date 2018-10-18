using Alaska.Foundation.Core.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Services
{
    internal class EntityContextBuilder<T>
    {
        public void Build()
        {
            using (new Profiler($"{nameof(EntityContextBuilder<T>)}"))
            {
            }
        }
    }
}
