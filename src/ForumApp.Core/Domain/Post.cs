using System;
using System.Collections.Generic;
using System.Text;

namespace ForumApp.Core.Domain
{
    public class Post : EntityBase
    {
        public string Id { get; set; }

        public string Content { get; set; }

        public string CreatorId { get; set; }

        public string TopicId { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? EditedOn { get; set; }

    }
}
