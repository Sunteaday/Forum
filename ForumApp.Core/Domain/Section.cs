using System;
using System.Collections.Generic;
using System.Text;

namespace ForumApp.Core.Domain
{
    public class Section : EntityBase
    {
        public string Id { get; set; }
        
        public string Name { get; set; }
    }
}
