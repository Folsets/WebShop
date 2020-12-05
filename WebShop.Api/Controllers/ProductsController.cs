using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebShop.Data.Entities;
using WebShop.Data.Interfaces;

namespace WebShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepo;

        public ProductsController(IProductRepository productRepo)
        {
            _productRepo = productRepo;
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpPost]
        public async Task<ActionResult<int>> Add(Product product)
        {
            var affectedRows = await _productRepo.Add(product);
            return affectedRows;
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> GetAll()
        {
            var products = await _productRepo.GetAll();
            return products;
        }
    }
}
