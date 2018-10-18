using Alaska.Foundation.Godzilla.Services;
using Alaska.Foundation.Godzilla.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alaska.Demo.WebApp.Infrastructure.Database
{
    public class AlaskaEntityContext : EntityContext
    {
        public AlaskaEntityContext(EntityContextOptions options)
            : base(options)
        { }
    }
}
