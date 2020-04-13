using Dapper;
using ForumApp.Core;
using ForumApp.Data.Infrastructure.Helpers.Reflection;
using ForumApp.Data.Infrastructure.Types;
using ForumApp.Data.Infrastructure.Types.Builders;
using ForumApp.Data.Repositories.Interfaces;
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
        protected string _insertProcedure;
        protected string _selectProcedure;
        protected string _selectAllProcedure;
        protected string _updateProcedure;
        protected string _deleteProcedure;

        protected IDbTransaction _dbTransaction;
        protected IDbConnection _dbConnection;
        #endregion

        public SqlRepository(SQLRepositoryBuilder builder)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            _insertProcedure = builder.InsertProcedure;
            _selectProcedure = builder.SelectProcedure;
            _selectAllProcedure = builder.SelectAllProcedure;
            _updateProcedure = builder.AlterProcedure;
            _deleteProcedure = builder.DeleteProcedure;

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
