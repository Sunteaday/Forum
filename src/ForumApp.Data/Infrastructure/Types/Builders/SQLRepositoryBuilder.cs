using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ForumApp.Data.Infrastructure.Types.Builders
{
    public class SqlRepositoryBuilder
    {
        public SqlRepositoryBuilder SetSelectProcedure(string selectProcedure)
        {
            if (string.IsNullOrWhiteSpace(selectProcedure))
                throw new ArgumentNullException(nameof(selectProcedure));

            SelectProcedure = selectProcedure;
            return this;
        }
        public SqlRepositoryBuilder SetSelectAllProcedure(string selectAllProcedure)
        {
            if (string.IsNullOrWhiteSpace(selectAllProcedure))
                throw new ArgumentNullException(nameof(selectAllProcedure));

            SelectAllProcedure = selectAllProcedure;
            return this;
        }

        public SqlRepositoryBuilder SetInsertProcedure(string insertProcedure)
        {
            if (string.IsNullOrWhiteSpace(insertProcedure))
                throw new ArgumentNullException(nameof(insertProcedure));

            InsertProcedure = insertProcedure;
            return this;
        }

        public SqlRepositoryBuilder SetDeleteProcedure(string deleteProcedure)
        {
            if (string.IsNullOrWhiteSpace(deleteProcedure))
                throw new ArgumentNullException(nameof(deleteProcedure));

            DeleteProcedure = deleteProcedure;
            return this;
        }

        public SqlRepositoryBuilder SetDeleteAllProcedure(string deleteAllprocedure)
        {
            if (string.IsNullOrWhiteSpace(deleteAllprocedure))
                throw new ArgumentNullException(nameof(deleteAllprocedure));

            DeleteAllProcedure = deleteAllprocedure;
            return this;
        }

        public SqlRepositoryBuilder SetAlterProcedure(string alterProcedure)
        {
            if (string.IsNullOrWhiteSpace(alterProcedure))
                throw new ArgumentNullException(nameof(alterProcedure));

            AlterProcedure = alterProcedure;
            return this;
        }

        public SqlRepositoryBuilder SetTransaction(IDbTransaction transaction)
        {
            Transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
            return this;
        }

        public string SelectProcedure { get; protected set; }

        public string SelectAllProcedure { get; protected set; }

        public string InsertProcedure { get; protected set; }

        public string DeleteProcedure { get; protected set; }

        public string DeleteAllProcedure { get; protected set; }

        public string AlterProcedure { get; protected set; }

        public IDbTransaction Transaction { get; protected set; }

    }
}
