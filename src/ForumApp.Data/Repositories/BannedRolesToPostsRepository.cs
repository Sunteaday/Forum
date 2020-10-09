using Dapper;
using ForumApp.Core.Domain.JunctionEntities;
using ForumApp.Core.Interfaces.Repositories;
using ForumApp.Data.Infrastructure.Types.Builders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace ForumApp.Data.Repositories
{
    public class BannedRolesToPostsRepository
        : SqlRepository<BannedRolesToPosts, BannedRolesToPosts>
        , IBannedRolesToPostsRepository

    {
        public BannedRolesToPostsRepository(SqlRepositoryBuilder builder)
            : base(builder)
        {

        }

        public override Task<BannedRolesToPosts> FindById(BannedRolesToPosts id)
        {
            return this.FindByIdInternal(new { RoleId = id.RoleId, PostId = id.PostId });
        }

        public override Task Remove(BannedRolesToPosts id)
        {
            return this.RemoveInternal(new { RoleId = id.RoleId, PostId = id.PostId });
        }
    }
}
