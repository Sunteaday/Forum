using Dapper;
using ForumApp.Core.Domain;
using ForumApp.Core.Interfaces.Repositories;
using ForumApp.Data.Infrastructure.Types.Builders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace ForumApp.Data.Repositories
{
    public class UserRepository 
        : SqlRepository<User, string>
        , IUserRepository
    {
        public UserRepository(SqlRepositoryBuilder builder)
            : base(builder)
        {

        }
    }
}