using Dapper;
using ForumApp.Core.Domain;
using ForumApp.Core.Interfaces.Repositories;
using ForumApp.Data.Infrastructure.Helpers.Reflection;
using ForumApp.Data.Infrastructure.Types;
using ForumApp.Data.Infrastructure.Types.Builders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumApp.Data.Repositories
{
    public class SqlRepository<TEntity, TId>
        : IRepository<TEntity, TId>
        where TEntity : EntityBase
    {
        #region Fields
        protected readonly string _insertProcedure;
        protected readonly string _selectProcedure;
        protected readonly string _selectAllProcedure;
        protected readonly string _alterProcedure;
        protected readonly string _deleteProcedure;
        protected readonly string _deleteAllProcedure;

        protected readonly IDbTransaction _dbTransaction;
        protected readonly IDbConnection _dbConnection;
        #endregion

        public SqlRepository(SqlRepositoryBuilder builder)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            _insertProcedure = builder.InsertProcedure;
            _selectProcedure = builder.SelectProcedure;
            _selectAllProcedure = builder.SelectAllProcedure;
            _alterProcedure = builder.AlterProcedure;
            _deleteProcedure = builder.DeleteProcedure;
            _deleteAllProcedure = builder.DeleteAllProcedure;

            _dbTransaction = builder.Transaction;
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
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            DynamicParameters parameters = CreateSqlArguments(entity);
            return _dbConnection.ExecuteAsync(
                sql: _insertProcedure
                , param: parameters
                , commandType: CommandType.StoredProcedure
                , transaction: _dbTransaction);

        }

        public virtual Task Update(TEntity entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            DynamicParameters parameters = CreateSqlArguments(entity);
            return _dbConnection.ExecuteAsync(
                  sql: _alterProcedure
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
        protected virtual Task RemoveInternal(object param)
        {
            return _dbConnection.ExecuteAsync(
                  sql: _deleteProcedure
                , param: param
                , commandType: CommandType.StoredProcedure
                , transaction: _dbTransaction);
        }
        public virtual Task Remove(TId id)
        {
            return RemoveInternal(new { Id = id });
        }
        protected virtual Task<TEntity> FindByIdInternal(object param)
        {
            return _dbConnection.QueryFirstAsync<TEntity>
               (sql: _selectProcedure
               , param: param
               , transaction: _dbTransaction
               , commandType: CommandType.StoredProcedure);
        }
        public virtual Task<TEntity> FindById(TId id)
        {
            return FindByIdInternal(new { Id = id });
        }

        public virtual Task RemoveAll()
        {
            return _dbConnection.ExecuteAsync(sql: _deleteAllProcedure);
        }
    }
}
