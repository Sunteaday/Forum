using Dapper;
using ForumApp.Core.Domain;
using ForumApp.Data.Helpers;
using ForumApp.Data.Helpers.Reflection;
using ForumApp.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumApp.Data.Repositories
{
    public class SqlRepository<TEntity, TId> : IRepository<TEntity, TId>
        where TEntity : EntityBase
    {
        private string _dbPrefix;
        private string _entityPrefix;
        private string _insertProcedure;
        private string _selectProcedure;
        private string _selectAllProcedure;
        private string _updateProcedure;
        private string _deleteProcedure;

        protected IDbTransaction _dbTransaction;
        protected IDbConnection _dbConnection;
        private void CreateProceduresNames()
        {
            // need to invert dependency
            _dbPrefix = "[dbo].[Forum";
            _entityPrefix = typeof(TEntity).Name;


            _insertProcedure = $"{_dbPrefix}_{_entityPrefix}_Insert]";
            _selectProcedure = $"{_dbPrefix}_{_entityPrefix}_Select]";
            _selectAllProcedure = $"{_dbPrefix}_{_entityPrefix}_SelectAll]";
            _updateProcedure = $"{_dbPrefix}_{_entityPrefix}_Update]";
            _deleteProcedure = $"{_dbPrefix}_{_entityPrefix}_Delete]";
        }
        public SqlRepository(IDbTransaction dbTransaction)
        {

            _dbTransaction = dbTransaction
            ?? throw new ArgumentNullException(nameof(dbTransaction));
            _dbConnection = _dbTransaction.Connection;

            CreateProceduresNames();
        }

        protected DynamicParameters CreateSqlArguments(TEntity entity)
        {
            var entityProps = entity.GetPropertiesAndValues();

            var parameters = new DynamicParameters();
            foreach (PropertyWrapper property in entityProps)
            {
                parameters.Add(
                   name: property.Name
                 , value: property.Value
                 , direction: ParameterDirection.Input
                 , size: (property.Value is string s) ? s.Length : (int?)null);
            }

            return parameters;
        }

        public virtual Task Add(TEntity entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));
            // Need to handle exesting entity

            DynamicParameters parameters = CreateSqlArguments(entity);
            return _dbConnection.ExecuteAsync(
                sql: _insertProcedure
                , param: parameters
                , commandType: CommandType.StoredProcedure
                , transaction: _dbTransaction);

        }

        public virtual Task Update(TEntity entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            DynamicParameters parameters = CreateSqlArguments(entity);

            return _dbConnection.ExecuteAsync(
                  sql: _updateProcedure
                , param: parameters
                , commandType: CommandType.StoredProcedure
                , transaction: _dbTransaction);
        }

        public virtual Task<IEnumerable<TEntity>> All()
        {
            return _dbConnection.QueryAsync<TEntity>
                (sql: _selectAllProcedure
                , transaction: _dbTransaction
                , commandType: CommandType.StoredProcedure);
        }
        public virtual Task Remove(TId id)
        {
            return _dbConnection.ExecuteAsync(
                  sql: _deleteProcedure
                , param: new { Id = id }
                , commandType: CommandType.StoredProcedure
                , transaction: _dbTransaction);
        }

        public virtual Task Remove(TEntity entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            TId id = FindPrimaryKey(entity);

            return Remove(id);
        }

        protected TId FindPrimaryKey(TEntity entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return entity
                        //GetPropertiesAndValues is too expensive for one property
                        .GetPropertiesAndValues()
                        .Where(p => p.Name.Equals("Id", StringComparison.InvariantCultureIgnoreCase)
                        && p.Type == typeof(TId))
                        .Select(p => p.Value)
                        .Cast<TId>()
                        .First();
        }

        public virtual Task<TEntity> FindById(TId id)
        {
            return _dbConnection.QueryFirstAsync<TEntity>
                (sql: _selectProcedure
                , param: new { Id = id }
                , transaction: _dbTransaction
                , commandType: CommandType.StoredProcedure);
        }
    }
}
