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

        SqlDataAccess _sql = new SqlDataAccess();

        public List<ProductModel> GetAllProduct()
        {
            var output = _sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new { }, "TRMData");
            return output;
        }

        public ProductModel GetProductById(int productId)
        {
            var output = _sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetById", new { Id = productId }, "TRMData").FirstOrDefault();
            return output;
        }
    }
}