
using System;
using System.Collections.Generic;
using System.Text;
using ForumApp.Core;
using ForumApp.Core.Domain;
using System.Reflection;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using System.Data;

namespace ForumApp.Data
{
    public class ForumUnitOfWork : IForumUnitOfWork, IDisposable
    {
        IDbConnection _dbConnection;
        IDbTransaction _dbTransaction;

        public ForumUnitOfWork(
            IDbConnection connection,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            _dbConnection = connection ?? throw new ArgumentNullException(nameof(connection));
            _dbTransaction = _dbConnection.BeginTransaction(isolationLevel);

        }
        public void Commit()
        {
            try
            {
                _dbTransaction.Commit();
            }
            catch (Exception ex)
            {
                _dbTransaction.Rollback();

                // is it a bit strange?
                throw new Exception(ex.Message);
            }
            finally
            {
                _dbTransaction.Dispose();
                _dbTransaction = _dbConnection.BeginTransaction();
            }
        }
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _dbConnection.Dispose();
                    _dbConnection = null;
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
