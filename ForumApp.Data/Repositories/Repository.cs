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
    public class Repository<TEntity, TId> : IRepository<TEntity, TId>
        where TEntity : EntityBase
    {
        protected IDbTransaction _dbTransaction;
        protected IDbConnection _dbConnection;
        public Repository(IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction
                ?? throw new ArgumentNullException(nameof(dbTransaction));
            _dbConnection = _dbTransaction.Connection;
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

            DynamicParameters parameters = CreateSqlArguments(entity);

            return _dbConnection.ExecuteAsync(
                "[dbo].[Forum_User_Insert]"
                , param: parameters
                , commandType: CommandType.StoredProcedure
                , transaction: _dbTransaction);

        }

        public virtual Task Update(TEntity entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            DynamicParameters parameters = CreateSqlArguments(entity);

            return _dbConnection.ExecuteAsync(
                "[dbo].[Forum_User_Update]"
                , param: parameters
                , commandType: CommandType.StoredProcedure
                , transaction: _dbTransaction);
        }

        public virtual Task<IEnumerable<TEntity>> All()
        {
            return _dbConnection.QueryAsync<TEntity>
                (sql: "Forum_User_SelectAll"
                , transaction: _dbTransaction
                , commandType: CommandType.StoredProcedure);
        }
        public virtual Task Remove(TId id)
        {
            return _dbConnection.ExecuteAsync(
                "[dbo].[Forum_User_Delete]"
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
                (sql: "Forum_User_Select"
                , param: new { Id = id }
                , transaction: _dbTransaction
                , commandType: CommandType.StoredProcedure);
        }
    }
}
