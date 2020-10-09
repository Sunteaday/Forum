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
    public class UserSettingRepository
        : SqlRepository<UserSetting, UserSetting>
        , IUserSettingRepository
    {
        public UserSettingRepository(SqlRepositoryBuilder builder)
            : base(builder)
        {
        }
        public override Task<UserSetting> FindById(UserSetting id)
        {
            return this.FindByIdInternal(new { SettingId = id.SettingId, UserId = id.UserId });
        }

        public override Task Remove(UserSetting id)
        {
            return this.RemoveInternal(new { SettingId = id.SettingId, UserId = id.UserId });
        }
    }
}
