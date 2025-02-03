using System;
using Dotnet.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotnetMastery.DataAccess.Repository.IRepository;
using System.Linq.Expressions;
using DotnetMastery.DataAccess.Data;

namespace DotnetMastery.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>,ICategoryRepository
    {

        private ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db):base(db) 
       
        {
            _db = db;
        }

      

        public void Update(Category obj)
        {
            _db.Categories.Update(obj);
        }
    }
}
