using Dapper;
using ForumApp.Core.Domain.JunctionEntities;
using ForumApp.Data.Infrastructure.Types.Builders;
using ForumApp.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace ForumApp.Data.Repositories
{
    public class UserForbiddenAbilityRepository
        : SqlRepository<UserForbiddenAbility, string[]>
        , IUserForbiddenAbilityRepository
    {
        public UserForbiddenAbilityRepository(SQLRepositoryBuilder builder)
            : base(builder)
        {
        }
        public override Task<UserForbiddenAbility> FindById(string[] id)
        {
            if (id == null || id.Length != 3)
                throw new ArgumentException(nameof(id));

            string userId = id[0];
            string abilityId = id[1];
            string topicId = id[2];

            return _dbConnection.QueryFirstAsync<UserForbiddenAbility>
                (sql: _selectProcedure
                , param: new
                {
                    UserId = userId,
                    AbilityId = abilityId,
                    TopicId = topicId
                }
                , transaction: _dbTransaction
                , commandType: CommandType.StoredProcedure);
        }
        public override Task Remove(string[] id)
        {
            if (id == null || id.Length != 3)
                throw new ArgumentException(nameof(id));

            string userId = id[0];
            string abilityId = id[1];
            string topicId = id[2];

            return _dbConnection.ExecuteAsync
                (sql: _deleteProcedure
                , param: new
                {
                    UserId = userId,
                    AbilityId = abilityId,
                    TopicId = topicId
                }
                , transaction: _dbTransaction
                , commandType: CommandType.StoredProcedure);
        }
    }
}
