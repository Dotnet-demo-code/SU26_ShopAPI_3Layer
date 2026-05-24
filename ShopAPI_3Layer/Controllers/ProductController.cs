using BusinessLayer.IServices;
using DataAccessLayer.DTOs.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ShopAPI_3Layer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var list = _productService.GetAllAsync().Result;
            return Ok(list);
        }

        [HttpPost]
        public IActionResult Post([FromBody] ProductAddDTO productCreateDTO)
        {
            var createdProduct = _productService.CreateAsync(productCreateDTO).Result;
            return CreatedAtAction(nameof(Get), new { id = createdProduct.ProductId }, createdProduct);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = _productService.GetByIdAsync(id).Result;
            if (product == null) return NotFound();
            return Ok(product);
        }
        [HttpPut]
        public IActionResult Put([FromBody] ProductDTO productUpdateDTO)
        {
            var updatedProduct = _productService.UpdateAsync(productUpdateDTO).Result;
            if (updatedProduct == null) return NotFound();
            return Ok(updatedProduct);
        }

        [HttpDelete]
        public IActionResult DeleteById(int id)
        {
            var deletedProduct = _productService.DeleteAsync(id).Result;
            if (deletedProduct == false) return NotFound();
            return NoContent();

        }
    }
}
