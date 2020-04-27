using ForumApp.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ForumApp.Core.Interfaces.Repositories
{
    public interface IRepository<TEntity, TId>
         where TEntity : EntityBase
    {
        public Task Add(TEntity entity);

        public Task<TEntity> FindById(TId id);

        public Task<IEnumerable<TEntity>> All();

        public Task Update(TEntity entity);

        public Task Remove(TId id);

        public Task RemoveAll();
    }
}
