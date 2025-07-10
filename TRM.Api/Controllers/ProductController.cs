using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TRMDataManager.Library.Internal.DataAccess;
using TRMDataManager.Library.Internal.Models;

namespace TRM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IProductData _productData;

        public ProductController(IConfiguration config, IProductData productData)
        {
            _config = config;
            _productData = productData;
        }

        
        

        // GET: api/Product
        [HttpGet]
        public List<ProductModel> Get()
        {
            return _productData.GetAllProduct();
        }
    }
}
