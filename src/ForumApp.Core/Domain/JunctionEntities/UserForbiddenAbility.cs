using System;
using System.Collections.Generic;
using System.Text;

namespace ForumApp.Core.Domain.JunctionEntities
{
    public class UserForbiddenAbility : EntityBase
    {
        public string UserId { get; set; }

        public string AbilityId { get; set; }

        public string TopicId { get; set; }
    }
}
