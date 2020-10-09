﻿using ForumApp.Core.Domain;
using ForumApp.Core.Interfaces.Repositories;
using ForumApp.Data.Infrastructure.Types.Builders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ForumApp.Data.Repositories
{
    public class PostRepository
        : SqlRepository<Post, string>
        , IPostRepository
    {
        public PostRepository(SqlRepositoryBuilder builder)
            : base(builder)
        {
        }
    }
}
