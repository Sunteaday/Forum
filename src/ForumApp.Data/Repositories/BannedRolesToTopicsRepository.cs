using Dapper;
using ForumApp.Core.Domain.JunctionEntities;
using ForumApp.Core.Interfaces.Repositories;
using ForumApp.Data.Infrastructure.Types.Builders;
using System;
using System.Data;
using System.Threading.Tasks;

namespace ForumApp.Data.Repositories
{
    public class BannedRolesToTopicsRepository
        : SqlRepository<BannedRolesToTopics, BannedRolesToTopics>
        , IBannedRolesToTopicsRepository


    {
        public BannedRolesToTopicsRepository(SqlRepositoryBuilder builder)
            : base(builder)
        {
        }

        public override Task<BannedRolesToTopics> FindById(BannedRolesToTopics id)
        {
            return this.FindByIdInternal(new { RoleId = id.RoleId, TopicId = id.TopicId });
        }

        public override Task Remove(BannedRolesToTopics id)
        {
            return this.RemoveInternal(new { RoleId = id.RoleId, TopicId = id.TopicId });
        }
    }
}
