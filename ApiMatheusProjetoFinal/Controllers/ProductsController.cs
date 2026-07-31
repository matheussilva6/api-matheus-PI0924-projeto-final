using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiMatheusProjetoFinal.Models;

namespace ApiMatheusProjetoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Isto obriga a usar o Token JWT para aceder a qualquer endpoint aqui
    public class ProductsController : ControllerBase
    {
        // Lista estática para simular a base de dados
        private static readonly List<Product> _products = new List<Product>
        {
            new Product { Id = 1, Name = "Computador Portátil HP", Price = 999.99m, Sku = "COMP-001" },
            new Product { Id = 2, Name = "Rato Wireless", Price = 25.50m, Sku = "RAT-002" }
        };

        // LER TODOS OS PRODUTOS
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_products);
        }

        // LER APENAS UM PRODUTO POR ID
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound(new { Message = "Produto não encontrado" });

            return Ok(product);
        }

        // CRIAR UM NOVO PRODUTO
        [HttpPost]
        public IActionResult Create([FromBody] Product newProduct)
        {
            newProduct.Id = _products.Count > 0 ? _products.Max(p => p.Id) + 1 : 1;
            _products.Add(newProduct);

            return CreatedAtAction(nameof(GetById), new { id = newProduct.Id }, newProduct);
        }

        // ATUALIZAR UM PRODUTO EXISTENTE
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Product updatedProduct)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound(new { Message = "Produto não encontrado" });

            product.Name = updatedProduct.Name;
            product.Price = updatedProduct.Price;
            product.Sku = updatedProduct.Sku;

            return Ok(product);
        }

        // ELIMINAR UM PRODUTO
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound(new { Message = "Produto não encontrado" });

            _products.Remove(product);
            return NoContent();
        }
    }
}