using Alaska.Foundation.Godzilla.Entries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class RelationshipAttribute : Attribute
    {
        private string _id;
        private RelationDirection _direction;

        public RelationshipAttribute(string id, RelationDirection direction)
        {
            _id = id;
            _direction = direction;
        }

        public string Id => _id;
        public RelationDirection Direction => _direction;
    }
}
