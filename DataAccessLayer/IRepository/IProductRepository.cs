using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.IRepository
{
    public interface IProductRepository
    {
        public Task<List<Product>> GetAllAsync();
        public Task<Product> GetByIdAsync(int id);
        public Task<Product> AddNewAsync(Product product);
        public Task<Product> UpdateAsync(Product product);
        public Task<Product> DeleteByIdAsync(int id);

        public Task<int> SaveChangeAsync();
    }
}
