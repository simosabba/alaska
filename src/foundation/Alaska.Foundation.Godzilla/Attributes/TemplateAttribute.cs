using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TemplateAttribute : Attribute
    {
        private string _id;

        public TemplateAttribute(string id)
        {
            _id = id;
        }
        
        public string Id => _id;
    }
}
