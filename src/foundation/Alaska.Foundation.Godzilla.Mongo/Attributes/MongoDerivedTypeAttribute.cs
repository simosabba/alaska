using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Mongo.Attributes
{
    public class MongoDerivedTypeAttribute : Attribute
    {
        private readonly string _name;

        public MongoDerivedTypeAttribute(string name)
        {
            _name = name;
        }

        public string Name => _name;
    }
}
