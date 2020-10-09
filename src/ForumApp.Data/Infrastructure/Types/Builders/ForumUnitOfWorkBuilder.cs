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
        private readonly Dictionary<Type, Func<object[], object>> _dependencies;

        public ForumUnitOfWorkBuilder()
        {
            _dependencies = new Dictionary<Type, Func<object[], object>>();
        }

        public ForumUnitOfWorkBuilder SetDependency<TInterface, TImplement>()
            where TInterface : class
            where TImplement : class, new()
        {
            _dependencies.Add(typeof(TInterface), _ => new TImplement());

            return this;
        }
        public ForumUnitOfWorkBuilder SetDependency<TInterface>(Func<object[], TInterface> implementationFactory)
           where TInterface : class
        {
            return SetDependency(typeof(TInterface), implementationFactory);
        }
        public ForumUnitOfWorkBuilder SetDependency(Type dependencyType, Func<object[], object> implementationFactory)
        {
            if (dependencyType is null || implementationFactory is null)
                throw new ArgumentNullException();

            _dependencies.Add(dependencyType, implementationFactory);

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
            return _dependencies[type];
        }
    }
}
