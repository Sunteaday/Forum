using ForumApp.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForumApp.Core.Interfaces.UnitsOfWork
{
    public interface IForumUnitOfWork : IDisposable
    {
        public void Commit();

        public void Rollback();

        IAbilityRepository Ability { get; }

        IBannedRolesToPostsRepository BannedRolesToPosts { get; }

        IBannedRolesToSectionRepository BannedRolesToSection { get; }

        IBannedRolesToTopicsRepository BannedRolesToTopics { get; }

        IModeratorTopicRepository ModeratorTopic { get; }

        IPostRepository Post { get; }

        IRoleRepository Role { get; }

        ISectionRepository Section { get; }

        ISettingRepository Setting { get; }

        ITopicRepository Topic { get; }

        IUserForbiddenAbilityRepository ForbiddenAbility { get; }

        IUserRepository User { get; }

        IUserSettingRepository UserSetting { get; }

    }
}
