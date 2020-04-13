using ForumApp.Core.Domain.JunctionEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForumApp.Data.Repositories.Interfaces
{
    public interface IBannedRolesToPostsRepository : IRepository<BannedRolesToPosts, string[]>
    {

    }
}
