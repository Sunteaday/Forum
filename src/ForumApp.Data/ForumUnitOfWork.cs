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
                var createdRepo = repoFactoryMethod.Invoke(new object[] { _dbTransaction });

                _repositories[repoType] = createdRepo;

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

        #region Repository Properties
        public IAbilityRepository Ability => _repositories[typeof(IAbilityRepository)] as IAbilityRepository;

        public IBannedRolesToPostsRepository BannedRolesToPosts => _repositories[typeof(IBannedRolesToPostsRepository)] as IBannedRolesToPostsRepository;

        public IBannedRolesToSectionRepository BannedRolesToSection => _repositories[typeof(IBannedRolesToSectionRepository)] as IBannedRolesToSectionRepository;

        public IBannedRolesToTopicsRepository BannedRolesToTopics => _repositories[typeof(IBannedRolesToTopicsRepository)] as IBannedRolesToTopicsRepository;

        public IModeratorTopicRepository ModeratorTopic => _repositories[typeof(IModeratorTopicRepository)] as IModeratorTopicRepository;

        public IPostRepository Post => _repositories[typeof(IPostRepository)] as IPostRepository;

        public IRoleRepository Role => _repositories[typeof(IRoleRepository)] as IRoleRepository;

        public ISectionRepository Section => _repositories[typeof(ISectionRepository)] as ISectionRepository;

        public ISettingRepository Setting => _repositories[typeof(ISettingRepository)] as ISettingRepository;

        public ITopicRepository Topic => _repositories[typeof(ITopicRepository)] as ITopicRepository;

        public IUserForbiddenAbilityRepository ForbiddenAbility => _repositories[typeof(IUserForbiddenAbilityRepository)] as IUserForbiddenAbilityRepository;

        public IUserRepository User => _repositories[typeof(IUserRepository)] as IUserRepository;

        public IUserSettingRepository UserSetting => _repositories[typeof(IUserSettingRepository)] as IUserSettingRepository;

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