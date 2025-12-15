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
        IQueryable<Entity> GetAll(Func<Entity, bool> condition = null);

        Entity? GetById(int Id);

        bool Create(Entity x);

        bool Update(Entity x);

        bool Delete(Entity x);
    }
}
