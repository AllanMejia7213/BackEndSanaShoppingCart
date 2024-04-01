using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SanaShoppingCart.Models;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Cors;

namespace SanaShoppingCart.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        public readonly SanaShoppingCartContext _dbContext;

        public ProductsController (SanaShoppingCartContext _context)
        {
            _dbContext = _context;
        }

        [HttpGet]
        [Route("ProductList")]
        public IActionResult GetProductList()
        {
            try
            {
                var productList = _dbContext.Products.Select(p => new {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Price = p.Price,
                    Stock = p.Stock,
                    ProductCode = p.ProductCode,
                    Categories = p.Categories.Select(c => new {
                        CategoryId = c.CategoryId,
                        CategoryName = c.CategoryName
                    })
                });

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Response = productList });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }

        [HttpGet]
        [Route("GetProductListByCategory/{idCategory:int}")]
        public IActionResult GetProductListByCategory(int idCategory)
        {
            List<Product> productList = new List<Product>();
            try
            {
                productList = _dbContext.Products.Where(p => p.Categories.Any(c => c.CategoryId == idCategory)).ToList();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Response = productList });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }

        [HttpPost]
        [Route("SaveProduct")]
        public IActionResult SaveProduct([FromBody] Product objeto)
        {
            try
            {
                _dbContext.Products.Add(objeto);
                _dbContext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }

        [HttpPut]
        [Route("EditProduct")]
        public IActionResult EditProduct([FromBody] Product objeto)
        {
            Product? oProduct = _dbContext.Products.Find(objeto.ProductId);
            if (oProduct == null)
            {
                return BadRequest("Product not found");
            }
            try
            {
                oProduct.ProductName = objeto.ProductName is null ? oProduct.ProductName : objeto.ProductName;
                oProduct.Description = objeto.Description is null ? oProduct.Description : objeto.Description;
                oProduct.Price = objeto.Price is null ? oProduct.Price : objeto.Price;
                _dbContext.Products.Update(oProduct);
                _dbContext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Response = oProduct });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }

        [HttpDelete]
        [Route("DeleteProduct/{productId:int}")]
        public IActionResult DeleteProduct(int productId)
        {
            Product? oProduct = _dbContext.Products.Find(productId);
            if (oProduct == null)
            {
                return BadRequest("Product not found");
            }
            try
            {
                _dbContext.Products.Remove(oProduct);
                _dbContext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }

    }
}
