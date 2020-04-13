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
    public class UserSettingRepository
        : SqlRepository<UserSetting, string[]>
        , IUserSettingRepository
    {
        public UserSettingRepository(SQLRepositoryBuilder builder)
            : base(builder)
        {
        }

        public override Task<UserSetting> FindById(string[] id)
        {
            if (id == null || id.Length != 2)
                throw new ArgumentException(nameof(id));

            string settingId = id[0];
            string userId = id[1];

            return _dbConnection.QueryFirstAsync<UserSetting>
                (sql: _selectProcedure
                , param: new { SettingId = settingId, UserId = userId }
                , transaction: _dbTransaction
                , commandType: CommandType.StoredProcedure);
        }
        public override Task Remove(string[] id)
        {
            if (id == null || id.Length != 2)
                throw new ArgumentException(nameof(id));

            string settingId = id[0];
            string userId = id[1];

            return _dbConnection.ExecuteAsync
                (sql: _deleteProcedure
                , param: new { SettingId = settingId, UserId = userId }
                , transaction: _dbTransaction
                , commandType: CommandType.StoredProcedure);
        }
    }
}
