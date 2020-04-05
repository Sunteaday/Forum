using System;
using System.Collections.Generic;
using System.Text;

namespace ForumApp.Core.Domain
{
    public class User : EntityBase
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Login { get; set; }

        public string PasswordHash { get; set; }

        public string Salt { get; set; }

        public string RoleId { get; set; }

    }
}
