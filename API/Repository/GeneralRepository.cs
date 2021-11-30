using API.Context;
using API.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository
{
    public class GeneralRepository<Context, Entity, Key> : IRepository<Entity, Key>
        where Entity : class
        where Context : MyContext
    {
        private readonly MyContext myContext;
        private readonly DbSet<Entity> entities;
        public GeneralRepository(MyContext myContext)
        {
            this.myContext = myContext;
            entities = myContext.Set<Entity>();
        }
        public int Insert(Entity entity)
        {
            var check = entities.Add(entity);
            if (check == null)
            {
                entities.Add(entity);
            }
            var result = myContext.SaveChanges();
            return result;
            /*entities.Add(entity);
            var result = myContext.SaveChanges();
            return result;*/
            //throw new NotImplementedException();
        }
        public int Delete(Key key)
        {
            var find = entities.Find(key);
            if (find != null)
            {
                myContext.Remove(find);
            }
            var result = myContext.SaveChanges();
            return result;
            //throw new NotImplementedException();
        }

        public IEnumerable<Entity> Get()
        {
            return entities.ToList();
            //throw new NotImplementedException();
        }

        public Entity Get(Key key)
        {
            return entities.Find(key);
            //throw new NotImplementedException();
        }

        public int Update(Entity entity, Key key)
        {
            myContext.Entry(entity).State = EntityState.Modified;
            var result = myContext.SaveChanges();
            return result;
            //throw new NotImplementedException();
        }
    }
}
