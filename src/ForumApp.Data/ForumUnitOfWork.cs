using System;
using System.Collections.Generic;
using System.Text;
using ForumApp.Core;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using ForumApp.Data.Repositories.Interfaces;
using ForumApp.Data.Infrastructure.Types.Builders;
using ForumApp.Data.Infrastructure.Helpers.Reflection;

namespace ForumApp.Data
{
    public class ForumUnitOfWork : IForumUnitOfWork, IDisposable
    {
        #region Fields
        IDictionary<Type, object> _repositories;
        IDictionary<Type, Func<object[], object>> _repositoryFactories;

        IDbConnection _dbConnection;
        IDbTransaction _dbTransaction;
        IsolationLevel _isolationLevel;
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
            // registering all IRepository<,> that defined in UnitOfWork class
            var _allReposTypes = this.GetType()
               .GetProperties(BindingFlags.Public | BindingFlags.Instance)
               .Where(pinf => pinf.PropertyType.DoesImplementGeneric(typeof(IRepository<,>)))
               .Select(p => p.PropertyType);


            foreach (Type repoType in _allReposTypes)
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

            foreach (var factory in _repositoryFactories)
            {
                // get factory result type
                Type repoType = factory.Key;

                // get factory by type
                var repoFactoryMethod = factory.Value;

                // create repo by factory
                var repository = repoFactoryMethod.Invoke(new object[] { _dbTransaction });

                _repositories[repoType] = repository;

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
        public IAbilityRepository Ability
        {
            get => _repositories[typeof(IAbilityRepository)] as IAbilityRepository;
        }
        public IBannedRolesToPostsRepository BannedRolesToPosts
        {
            get => _repositories[typeof(IBannedRolesToPostsRepository)] as IBannedRolesToPostsRepository;
        }
        public IBannedRolesToSectionRepository BannedRolesToSection
        {
            get => _repositories[typeof(IBannedRolesToSectionRepository)] as IBannedRolesToSectionRepository;
        }
        public IBannedRolesToTopicsRepository BannedRolesToTopics
        {
            get => _repositories[typeof(IBannedRolesToTopicsRepository)] as IBannedRolesToTopicsRepository;
        }
        public IModeratorTopicRepository ModeratorTopic
        {
            get => _repositories[typeof(IModeratorTopicRepository)] as IModeratorTopicRepository;
        }
        public IPostRepository Post
        {
            get => _repositories[typeof(IPostRepository)] as IPostRepository;
        }
        public IRoleRepository Role
        {
            get => _repositories[typeof(IRoleRepository)] as IRoleRepository;
        }
        public ISectionRepository Section
        {
            get => _repositories[typeof(ISectionRepository)] as ISectionRepository;
        }
        public ISettingRepository Setting
        {
            get => _repositories[typeof(ISettingRepository)] as ISettingRepository;
        }
        public ITopicRepository Topic
        {
            get => _repositories[typeof(ITopicRepository)] as ITopicRepository;
        }
        public IUserForbiddenAbilityRepository ForbiddenAbility
        {
            get => _repositories[typeof(IUserForbiddenAbilityRepository)] as IUserForbiddenAbilityRepository;
        }
        public IUserRepository User
        {
            get => _repositories[typeof(IUserRepository)] as IUserRepository;
        }
        public IUserSettingRepository UserSetting
        {
            get => _repositories[typeof(IUserSettingRepository)] as IUserSettingRepository;
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this._dbTransaction.Dispose();
                    this._dbTransaction = null;

                    this._dbConnection.Dispose();
                    this._dbConnection = null;

                    this._repositories = null;
                    this._repositoryFactories = null;
                }

                disposedValue = true;
            }
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