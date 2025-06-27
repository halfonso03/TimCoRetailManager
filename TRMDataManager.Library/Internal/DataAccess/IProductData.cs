using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Library.Internal.DataAccess
{
    public interface IProductData
    {
        List<ProductModel> GetAllProduct();
        ProductModel GetProductById(int productId);
    }
}
