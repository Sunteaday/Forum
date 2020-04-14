using ForumApp.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ForumApp.Data.Infrastructure.Types.Builders
{
    // Probably need to create a Build() method that invalidates all inputs and returns a FinalizedForumUnitOfWorkBuilder
    public class ForumUnitOfWorkBuilder
    {
        Dictionary<Type, Func<object[], object>> _dependecies = new Dictionary<Type, Func<object[], object>>();

        public ForumUnitOfWorkBuilder SetDependency(Type interfaceType, Func<object[], object> implementationFactory)
        {
            if (interfaceType is null)
                throw new ArgumentNullException(nameof(interfaceType));

            if (implementationFactory is null)
                throw new ArgumentNullException(nameof(implementationFactory));


            _dependecies.Add(interfaceType, implementationFactory);

            return this;
        }
        public IDbConnection DbConnection { get; protected set; }
        public IsolationLevel? IsolationLevel { get; protected set; } = System.Data.IsolationLevel.ReadCommitted;

        public ForumUnitOfWorkBuilder SetDbConnection(IDbConnection dbConnection)
        {
            DbConnection = dbConnection
                ?? throw new ArgumentNullException(nameof(dbConnection));

            return this;
        }

        public ForumUnitOfWorkBuilder SetIsolationLevel(IsolationLevel? isolationLevel)
        {
            IsolationLevel = isolationLevel ?? IsolationLevel;

            return this;
        }

        public Func<object[], object> ResolveDependency(Type type)
        {
            return _dependecies[type];
        }
    }
}
