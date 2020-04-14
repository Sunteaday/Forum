using Dapper;
using ForumApp.Core.Domain.JunctionEntities;
using ForumApp.Data.Infrastructure.Types.Builders;
using ForumApp.Data.Repositories.Interfaces;
using System;
using System.Data;
using System.Threading.Tasks;

namespace ForumApp.Data.Repositories
{
    public class BannedRolesToTopicsRepository
        : SqlRepository<BannedRolesToTopics, string[]>
        , IBannedRolesToTopicsRepository
    {
        public BannedRolesToTopicsRepository(SQLRepositoryBuilder builder)
            : base(builder)
        {
        }

        public override Task<BannedRolesToTopics> FindById(string[] id)
        {
            if (id == null || id.Length != 2)
                throw new ArgumentException(nameof(id));

            string roleId = id[0];
            string topicId = id[1];

            return _dbConnection.QueryFirstAsync<BannedRolesToTopics>
                (sql: _selectProcedure
                , param: new { RoleId = roleId, TopicId = topicId }
                , transaction: _dbTransaction
                , commandType: CommandType.StoredProcedure);
        }
        public override Task Remove(string[] id)
        {
            if (id == null || id.Length != 2)
                throw new ArgumentException(nameof(id));

            string roleId = id[0];
            string topicId = id[1];

            return _dbConnection.ExecuteAsync
                (sql: _deleteProcedure
                , param: new { RoleId = roleId, TopicId = topicId }
                , transaction: _dbTransaction
                , commandType: CommandType.StoredProcedure);
        }
    }
}
