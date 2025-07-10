using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDataManager.Library.Internal.DataAccess;
using TRMDataManager.Library.Internal.Models;

namespace TRMDataManager.Library.Internal.DataAccess
{
    public class ProductData : IProductData
    {

        private readonly IConfiguration _config;

        public ProductData(IConfiguration config)
        {
            _config = config;

        }

        

        public List<ProductModel> GetAllProduct()
        {
            SqlDataAccess _sql = new SqlDataAccess(_config);
            var output = _sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new { }, "TRMData");
            return output;
        }

        public ProductModel GetProductById(int productId)
        {
            SqlDataAccess _sql = new SqlDataAccess(_config);
            var output = _sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetById", new { Id = productId }, "TRMData").FirstOrDefault();
            return output;
        }
    }
}