using GymManagementDAL.Context;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Class
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<Type, object> _Repositories = new();
        private readonly GymContext _dbContext;
        public UnitOfWork(GymContext dbContext, ISessionRepository sessionRepository)
        {
            _dbContext = dbContext;
            SessionRepository = sessionRepository;
        }

        public ISessionRepository SessionRepository { get; }

        public IGenericRepository<Entity> GetRepository<Entity>() where Entity : BaseEntity, new()
        {
            var entityType = typeof(Entity);
            if(_Repositories.TryGetValue(entityType, out var repository))
            {
                return (IGenericRepository<Entity>)repository;
            }
            else
            {
                var newRepo = new GenericRepository<Entity>(_dbContext);
                _Repositories[entityType] = newRepo;
                return newRepo;
            }
            
        }

        public int SaveChange()
        {
            return _dbContext.SaveChanges();
        }
    }
}
