using System;
using System.Collections.Generic;
using System.Text;

namespace ForumApp.Core.Domain.JunctionEntities
{
    public class ModeratorTopic : EntityBase
    {
        public string UserId { get; set; }

        public string TopicId { get; set; }
    }
}
