using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Settings
{
    public class DatabaseCollectionOptions
    {
        public string ConnectionString { get; set; }

        public Type ProviderType { get; set; }

        public bool DisableLogs { get; set; } = false;
    }
}
