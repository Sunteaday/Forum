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
    public class UserForbiddenAbilityRepository
        : SqlRepository<UserForbiddenAbility, UserForbiddenAbility>
        , IUserForbiddenAbilityRepository
    {
        public UserForbiddenAbilityRepository(SqlRepositoryBuilder builder)
            : base(builder)
        {
        }

        public override Task<UserForbiddenAbility> FindById(UserForbiddenAbility id)
        {
            return this.FindByIdInternal(new
                {
                    UserId = id.UserId,
                    AbilityId = id.AbilityId,
                    TopicId = id.TopicId
                });
        }

        public override Task Remove(UserForbiddenAbility id)
        {
            return this.RemoveInternal(new
                {
                    UserId = id.UserId,
                    AbilityId = id.AbilityId,
                    TopicId = id.TopicId
                });
        }        
    }
}
