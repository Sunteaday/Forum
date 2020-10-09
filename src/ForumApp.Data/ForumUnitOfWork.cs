using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using ForumApp.Data.Infrastructure.Types.Builders;
using ForumApp.Core.Interfaces.UnitsOfWork;
using ForumApp.Core.Interfaces.Repositories;
using ForumApp.Data.Infrastructure.Helpers.Reflection;

namespace ForumApp.Data
{
    public class ForumUnitOfWork : IForumUnitOfWork
    {
        #region Fields

        private IDictionary<Type, object> _repositories;
        private IDictionary<Type, Func<object[], object>> _repositoryFactories;

        private IDbConnection _dbConnection;
        private IDbTransaction _dbTransaction;
        private IsolationLevel _isolationLevel;
        #endregion

        #region Private Methods
        private void InitializeFields(ForumUnitOfWorkBuilder builder)
        {
            _repositories = new Dictionary<Type, object>();
            _repositoryFactories = new Dictionary<Type, Func<object[], object>>();

            _dbConnection = builder.DbConnection;
            if (_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();

            _isolationLevel = builder.IsolationLevel ?? IsolationLevel.ReadCommitted;

        }

        private void RegisterRepositories(ForumUnitOfWorkBuilder builder)
        {
            // gather all repositories that implement IRepository in our UnitOfWork class
            var allReposTypes = this.GetType()
               .GetProperties(BindingFlags.Public | BindingFlags.Instance)
               .Where(pi => pi.PropertyType.DoesImplementGeneric(typeof(IRepository<,>)))
               .Select(p => p.PropertyType);


            foreach (Type repoType in allReposTypes)
            {
                // resolving repos factories from our builder
                var repoFactory = builder.ResolveDependency(repoType);
                // get and save repo factory for recreate repo 
                _repositoryFactories.Add(repoType, repoFactory);
            }
        }

        private void ResetRepositories()
        {
            _dbTransaction?.Dispose();
            _dbTransaction = _dbConnection.BeginTransaction(_isolationLevel);

            foreach (var (repoType, repoFactoryMethod) in _repositoryFactories)
            {
                var repositoryInsance = repoFactoryMethod.Invoke(new object[] { _dbTransaction });

                _repositories[repoType] = repositoryInsance;

            }
        }
        #endregion

        public ForumUnitOfWork(ForumUnitOfWorkBuilder builder)
        {
            this.InitializeFields(builder);
            this.RegisterRepositories(builder);
            this.ResetRepositories();
        }

        public void Commit()
        {
            try
            {
                this._dbTransaction.Commit();
            }
            catch (Exception ex)
            {
                this._dbTransaction.Rollback();

                // is it a bit strange?
                throw new Exception(ex.Message);
            }
            finally
            {
                this.ResetRepositories();
            }
        }

        public void Rollback()
        {
            this._dbTransaction.Rollback();

            ResetRepositories();
        }

        private T ResolveRepositoryByType<T>()
            where T : class
        {
            _repositories.TryGetValue(typeof(T), out object repositoryInstance);
            return repositoryInstance as T ?? throw new ArgumentOutOfRangeException(nameof(T));
        }

        #region Repository Properties
        public IAbilityRepository Ability => ResolveRepositoryByType<IAbilityRepository>();

        public IBannedRolesToPostsRepository BannedRolesToPosts => ResolveRepositoryByType<IBannedRolesToPostsRepository>();

        public IBannedRolesToSectionRepository BannedRolesToSection => ResolveRepositoryByType<IBannedRolesToSectionRepository>();

        public IBannedRolesToTopicsRepository BannedRolesToTopics => ResolveRepositoryByType<IBannedRolesToTopicsRepository>();

        public IModeratorTopicRepository ModeratorTopic => ResolveRepositoryByType<IModeratorTopicRepository>();

        public IPostRepository Post => ResolveRepositoryByType<IPostRepository>();

        public IRoleRepository Role => ResolveRepositoryByType<IRoleRepository>();

        public ISectionRepository Section => ResolveRepositoryByType<ISectionRepository>();

        public ISettingRepository Setting => ResolveRepositoryByType<ISettingRepository>();

        public ITopicRepository Topic => ResolveRepositoryByType<ITopicRepository>();

        public IUserForbiddenAbilityRepository ForbiddenAbility => ResolveRepositoryByType<IUserForbiddenAbilityRepository>();

        public IUserRepository User => ResolveRepositoryByType<IUserRepository>();

        public IUserSettingRepository UserSetting => ResolveRepositoryByType<IUserSettingRepository>();

        #endregion

        #region IDisposable Support
        private bool _isDisposed = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed || !disposing)
                return;

            this._dbTransaction.Dispose();
            this._dbTransaction = null;

            this._dbConnection.Dispose();
            this._dbConnection = null;

            this._repositories = null;
            this._repositoryFactories = null;

            _isDisposed = true;
        }
        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);

            //GC.SuppressFinalize(this);
        }
        #endregion
    }
}