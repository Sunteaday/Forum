using System;
using System.Collections.Generic;
using System.Text;

namespace ForumApp.Core.Domain.JunctionEntities

{
    public class BannedRolesToPosts : EntityBase
    {
        public string RoleId { get; set; }

        public string PostId { get; set; }
    }
}
