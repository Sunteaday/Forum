using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ForumApp.Data.Infrastructure.Types.Builders
{
    public class SQLRepositoryBuilder
    {
        public SQLRepositoryBuilder SetSelectProcedure(string selectProcedure)
        {
            if (string.IsNullOrWhiteSpace(selectProcedure))
                throw new ArgumentNullException(nameof(selectProcedure));

            SelectProcedure = selectProcedure;
            return this;
        }
        public SQLRepositoryBuilder SetSelectAllProcedure(string selectAllProcedure)
        {
            if (string.IsNullOrWhiteSpace(selectAllProcedure))
                throw new ArgumentNullException(nameof(selectAllProcedure));

            SelectAllProcedure = selectAllProcedure;
            return this;
        }

        public SQLRepositoryBuilder SetInsertProcedure(string insertProcedure)
        {
            if (string.IsNullOrWhiteSpace(insertProcedure))
                throw new ArgumentNullException(nameof(insertProcedure));

            InsertProcedure = insertProcedure;
            return this;
        }

        public SQLRepositoryBuilder SetDeleteProcedure(string deleteProcedure)
        {
            if (string.IsNullOrWhiteSpace(deleteProcedure))
                throw new ArgumentNullException(nameof(deleteProcedure));

            DeleteProcedure = deleteProcedure;
            return this;
        }

        public SQLRepositoryBuilder SetAlterProcedure(string alterProcedure)
        {
            if (string.IsNullOrWhiteSpace(alterProcedure))
                throw new ArgumentNullException(nameof(alterProcedure));

            AlterProcedure = alterProcedure;
            return this;
        }

        public SQLRepositoryBuilder SetTransaction(IDbTransaction transaction)
        {
            if (transaction is null)
                throw new ArgumentNullException(nameof(transaction));

            Transaction = transaction;
            return this;
        }

        public string SelectProcedure { get; protected set; }

        public string SelectAllProcedure { get; protected set; }

        public string InsertProcedure { get; protected set; }

        public string DeleteProcedure { get; protected set; }

        public string AlterProcedure { get; protected set; }

        public IDbTransaction Transaction { get; protected set; }

    }
}
