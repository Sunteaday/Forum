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
        : SqlRepository<BannedRolesToSection, string[]>
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
        public override Task<BannedRolesToSection> FindById(string[] id)
        {
            if (id == null || id.Length != 2)
                throw new ArgumentException(nameof(id));

            string roleId = id[0];
            string sectionId = id[1];

            return _dbConnection.QueryFirstAsync<BannedRolesToSection>
                (sql: _selectProcedure
                , param: new { RoleId = roleId, SectionId = sectionId }
                , transaction: _dbTransaction
                , commandType: CommandType.StoredProcedure);
        }
        public override Task Remove(string[] id)
        {
            if (id == null || id.Length != 2)
                throw new ArgumentException(nameof(id));

            string roleId = id[0];
            string sectionId = id[1];

            return _dbConnection.ExecuteAsync
                (sql: _deleteProcedure
                , param: new { RoleId = roleId, SectionId = sectionId }
                , transaction: _dbTransaction
                , commandType: CommandType.StoredProcedure);
        }
    }
}
