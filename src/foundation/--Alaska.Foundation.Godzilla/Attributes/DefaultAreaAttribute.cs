using Alaska.Foundation.Godzilla.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Alaska.Foundation.Godzilla.Attributes
{
    public class DefaultAreaAttribute : Attribute
    {
        private string _path;

        public DefaultAreaAttribute(string path)
        {
            _path = path;
        }

        public string Path => _path;
    }
}
