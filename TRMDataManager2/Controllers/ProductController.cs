using System.Collections.Generic;
using System.Web.Http;
using TRMDataManager.Library.Internal.DataAccess;
using TRMDataManager.Library.Internal.Models;

namespace TRMDataManager.Controllers
{
    [Route("api/Product")]
    [Authorize(Roles = "Cashier")]
    public class ProductController : ApiController
    {
        private readonly ProductData _productData = new ProductData();

        //public ProductController(IProductData productData)
        //{
        //    _productData = productData;
        //}

        // GET: api/Product
        [HttpGet]
        public List<ProductModel> Get()
        {
            return _productData.GetAllProduct();
        }
    }
}