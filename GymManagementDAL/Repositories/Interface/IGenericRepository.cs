using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interface
{
    public interface IGenericRepository<Entity> where Entity : BaseEntity, new()
    {
        IQueryable<Entity> GetAll(Func<Entity, bool> condition = null!);

        Entity? GetById(int Id);

        void Create(Entity x);

        void Update(Entity x);

        void Delete(Entity x);
    }
}
