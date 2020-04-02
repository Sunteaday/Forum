using System;
using System.Collections.Generic;
using System.Text;

namespace ForumApp.Core.Domain.JunctionEntities
{
    public class UserSetting : EntityBase
    {        
        public string SettingId { get; set; }
        
        public string UserId { get; set; }
        
        public string Value { get; set; }
    }
}
