using DataAccessLayer.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.IServices
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetAllAsync();
        Task<ProductDTO> GetByIdAsync(int id);
        Task<ProductDTO> CreateAsync(ProductAddDTO productCreateDTO);
        Task<ProductDTO> UpdateAsync(ProductDTO productUpdateDTO);
        Task<bool> DeleteAsync(int id);
    }
}
