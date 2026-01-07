using GymManagementDAL.Context;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Class
{
    public class GenericRepository<Entity> : IGenericRepository<Entity> where Entity : BaseEntity, new()
    {
        private readonly GymContext _context;
        public GenericRepository(GymContext context)
        {
            _context = context;
        }

        public void Create(Entity x)=>_context.Set<Entity>().Add(x);

        public void Delete(Entity x) => _context.Set<Entity>().Remove(x);


        public IQueryable<Entity> GetAll(Func<Entity, bool> condition = null!)
        {
            List<Entity> list = new List<Entity>();
            if (condition == null)
            {
                list = _context.Set<Entity>().AsNoTracking().ToList();
            }
            else
            {
                list = _context.Set<Entity>().AsNoTracking().Where(condition).ToList();
            }
            return list.AsQueryable();

        }

        public Entity? GetById(int Id)=>_context.Set<Entity>().Find(Id);

        public void Update(Entity x)=>_context.Set<Entity>().Update(x);
    }
}
