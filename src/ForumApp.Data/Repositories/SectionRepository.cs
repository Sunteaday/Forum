﻿using ForumApp.Core.Domain;
using ForumApp.Data.Infrastructure.Types.Builders;
using ForumApp.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ForumApp.Data.Repositories
{
    public class SectionRepository
        : SqlRepository<Section, string>
        , ISectionRepository
    {
        public SectionRepository(SQLRepositoryBuilder builder)
            : base(builder)
        {
        }
    }
}