﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShop.Core.Contracts;
using MyShop.Core.Models;

namespace MyShop.DataAccess.SQL
{
    public class SQLRepositories<T> : IRepository<T> where T : BaseEntity
    {

        internal DataContext context;
        internal DbSet<T> dbSet;

        public SQLRepositories(DataContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>(); 
        }
        IQueryable<T> IRepository<T>.Collection()
        {
            return dbSet;
        }

        public void Commit()
        {
            context.SaveChanges();
        }

        public void Delete(string Id)
        {
            var t = Find(Id);
            if (context.Entry(t).State == EntityState.Detached) { 
                dbSet.Attach(t);
            }
            dbSet.Remove(t);
        }

        public T Find(string Id)
        {
            return dbSet.Find(Id);
        }

        public void Insert(T t)
        {
            dbSet.Add(t);
        }

        public void Update(T t)
        {
            dbSet.Attach(t);
            context.Entry(t).State = EntityState.Modified;
        }
    }
}
