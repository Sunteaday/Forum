using System;
using System.Collections.Generic;
using System.Text;

namespace ForumApp.Data.Infrastructure.Types
{
    internal class PropertyWrapper
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public Type Type { get; set; }

    }
}
