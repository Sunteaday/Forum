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
        : SqlRepository<ModeratorTopic, string[]>
        , IModeratorTopicRepository
    {
        public ModeratorTopicRepository(SqlRepositoryBuilder builder)
            : base(builder)
        {
        }

        public override Task<ModeratorTopic> FindById(string[] id)
        {
            if (id == null || id.Length != 2)
                throw new ArgumentException(nameof(id));

            string userId = id[0];
            string topicId = id[1];

            return _dbConnection.QueryFirstAsync<ModeratorTopic>
                (sql: _selectProcedure
                , param: new { UserId = userId, TopicId = topicId }
                , transaction: _dbTransaction
                , commandType: CommandType.StoredProcedure);
        }
        public override Task Remove(string[] id)
        {
            if (id == null || id.Length != 2)
                throw new ArgumentException(nameof(id));

            string userId = id[0];
            string topicId = id[1];

            return _dbConnection.ExecuteAsync
                (sql: _deleteProcedure
                , param: new { UserId = userId, TopicId = topicId }
                , transaction: _dbTransaction
                , commandType: CommandType.StoredProcedure);
        }
    }
}
