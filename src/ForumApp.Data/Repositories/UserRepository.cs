﻿using Dapper;
using ForumApp.Core.Domain;
using ForumApp.Data.Infrastructure.Types.Builders;
using ForumApp.Data.Repositories.Interfaces;
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
        public UserRepository(SQLRepositoryBuilder builder)
            : base(builder)
        {

        }
    }
}