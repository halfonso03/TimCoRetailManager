using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using TRMDataManager.Library.Internal.DataAccess;
using TRMDataManager.Library.Internal.Models;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Library.DataAccess
{
    public class SaleData : ISaleData, IDisposable
    {
        private readonly IConfiguration _config;
        private readonly IProductData _productData;
        private readonly ISqlDataAccess _sqlDataAccess;

        public SaleData(IConfiguration config, IProductData productData, ISqlDataAccess sqlDataAccess)
        {
            _config = config;
            _productData = productData;
            _sqlDataAccess = sqlDataAccess;
        }

        public void SaveSale(SaleModel saleInfo, string cashierId)
        {
            // TODO:: Make this SOLID/DRY/Better
            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();
            var taxRate = ConfigHelper.GetTaxRate(_config) / 100;

            foreach (var item in saleInfo.SaleDetails)
            {
                var detail = new SaleDetailDBModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                // Get information about this product
                var productInfo = _productData.GetProductById(detail.ProductId);

                if (productInfo == null)
                {
                    throw new Exception($"The product Id of {detail.ProductId} could not be found in the database");
                }

                detail.PurchasePrice = productInfo.RetailPrice * detail.Quantity;

                if (productInfo.IsTaxable)
                {
                    detail.Tax = (detail.PurchasePrice * taxRate);
                }

                details.Add(detail);

            }

            // Create the sale model
            SaleDBModel sale = new SaleDBModel
            {
                SubTotal = details.Sum(x => x.PurchasePrice),
                Tax = details.Sum(x => x.Tax),
                CashierId = cashierId
            };

            sale.Total = sale.SubTotal + sale.Tax;

            try
            {
                _sqlDataAccess.StartTransaction("TRMData");
                //Save the model
                _sqlDataAccess.SaveDataInTransaction<SaleDBModel>("dbo.spSale_Insert", sale);

                // Get the Id from the sale model
                sale.Id = _sqlDataAccess.LoadDataInTransaction<int, dynamic>("dbo.spSale_Lookup", new { CashierId = sale.CashierId, SaleDate = sale.SaleDate }).FirstOrDefault();

                // Finish filling in the sale details models
                foreach (var item in details)
                {
                    item.SaleId = sale.Id;

                    // Save the sale details models
                    _sqlDataAccess.SaveDataInTransaction("dbo.spSaleDetail_Insert", item);
                }
                //Can explicitly call but it will implicitly close after using statment finished.
                _sqlDataAccess.CommitTransaction();
            }
            catch
            {
                _sqlDataAccess.RollbackTransaction();
                throw;
            }
        }


        public List<SaleReportModel> GetSaleReport()
        {
            var output = _sqlDataAccess.LoadData<SaleReportModel, dynamic>("dbo.spSale_SaleReport", new { }, "TRMData");
            return output;
        }

        public void Dispose()
        {
            _sqlDataAccess?.Dispose();            
        }
    }
}