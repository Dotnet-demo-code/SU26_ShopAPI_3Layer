
using BusinessLayer.IServices;
using DataAccessLayer.DTOs.Product;
using DataAccessLayer.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductDTO>> GetAllAsync()
        {
            var list = await _productRepository.GetAllAsync();

            // Logic filter, sort, paging, etc.

            return list.Select(p => new ProductDTO
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Price = p.Price,
                Stock = p.Stock
            }).ToList();
        }

        public async Task<ProductDTO> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return null;
            return new ProductDTO
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock
            };
        }
        public async Task<ProductDTO> CreateAsync(ProductAddDTO productCreateDTO)
        {
            // validation before mapping
            if (string.IsNullOrEmpty(productCreateDTO.Name))
            {
                throw new ArgumentException("Product name is required.");
            }

            if (productCreateDTO.Price <= 0)
            {
                throw new ArgumentException("Price cannot be negative.");
            }

            if (productCreateDTO.Stock < 0)
            {
                throw new ArgumentException("Stock cannot be negative.");
            }
            var product = new DataAccessLayer.Models.Product
            {
                Name = productCreateDTO.Name,
                Price = productCreateDTO.Price,
                Stock = productCreateDTO.Stock
            };
            var createdProduct = await _productRepository.AddNewAsync(product);
            return new ProductDTO
            {
                ProductId = createdProduct.ProductId,
                Name = createdProduct.Name,
                Price = createdProduct.Price,
                Stock = createdProduct.Stock
            };
        }

        public async Task<ProductDTO> UpdateAsync(ProductDTO productUpdateDTO)
        {
            var product = new DataAccessLayer.Models.Product
            {
                ProductId = productUpdateDTO.ProductId,
                Name = productUpdateDTO.Name,
                Price = productUpdateDTO.Price,
                Stock = productUpdateDTO.Stock
            };
            var updatedProduct = await _productRepository.UpdateAsync(product);
            return new ProductDTO
            {
                ProductId = updatedProduct.ProductId,
                Name = updatedProduct.Name,
                Price = updatedProduct.Price,
                Stock = updatedProduct.Stock
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deletedProduct = await _productRepository.DeleteByIdAsync(id);
            return deletedProduct != null;
        }
    }
}
