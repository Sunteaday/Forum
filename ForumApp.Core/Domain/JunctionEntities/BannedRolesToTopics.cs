using System;
using System.Collections.Generic;
using System.Text;

namespace ForumApp.Core.Domain.JunctionEntities
{
    public class BannedRolesToTopics : EntityBase
    {
        
        public string RoleId { get; set; }
        public string TopicId { get; set; }
    }
}
