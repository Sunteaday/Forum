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
    public class ModeratorTopicRepository
        : SqlRepository<ModeratorTopic, ModeratorTopic>
        , IModeratorTopicRepository
    {
        public ModeratorTopicRepository(SqlRepositoryBuilder builder)
            : base(builder)
        {

        }

        public override Task<ModeratorTopic> FindById(ModeratorTopic id)
        {
            return this.FindByIdInternal(new { UserId = id.UserId, TopicId = id.TopicId });
        }

        public override Task Remove(ModeratorTopic id)
        {
            return this.RemoveInternal(new { UserId = id.UserId, TopicId = id.TopicId });
        }

    }
}
