using ForumApp.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForumApp.Data
{
    public interface IForumUnitOfWork
    {
        void Commit();

        void Rollback();

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
