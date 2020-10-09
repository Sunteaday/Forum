using Dapper;
using ForumApp.Core.Domain.JunctionEntities;
using ForumApp.Core.Interfaces.Repositories;
using ForumApp.Data.Infrastructure.Types.Builders;
using System;
using System.Data;
using System.Threading.Tasks;

namespace ForumApp.Data.Repositories
{
    public class BannedRolesToSectionRepository
        : SqlRepository<BannedRolesToSection, BannedRolesToSection>
        , IBannedRolesToSectionRepository
    {
        public BannedRolesToSectionRepository(SqlRepositoryBuilder builder)
            : base(builder)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Warning. A strict usage pass ids as defined in the class</param>
        /// <returns></returns>
        public override Task<BannedRolesToSection> FindById(BannedRolesToSection id)
        {
            return this.FindByIdInternal(new { RoleId = id.RoleId, SectionId = id.SectionId });
        }

        public override Task Remove(BannedRolesToSection id)
        {
            return this.RemoveInternal(new { RoleId = id.RoleId, SectionId = id.SectionId });
        }
    }
}
