using Caliburn.Micro;
using System.Collections.ObjectModel;
using System.Linq;
using TRMDesktopUI.Library.Api;
using TRMDesktopUI.Library.Helpers;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private readonly IProductEndpoint _productEndpoint;
        private readonly IConfigHelper _configHelper;
        private readonly ISaleEndpoint _saleEndpoint;

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            var p = await _productEndpoint.GetAll();
            //Products = new BindingList<ProductModel>(p);

            p.ForEach(x =>
            {
                Products.Add(x);
            });
        }

        public SalesViewModel(IProductEndpoint productEndpoint, IConfigHelper configHelper, ISaleEndpoint saleEndpoint)
        {
            _productEndpoint = productEndpoint;
            _configHelper = configHelper;
            _saleEndpoint = saleEndpoint;
        }

        private ObservableCollection<ProductModel> _products = new ObservableCollection<ProductModel>();

        public ObservableCollection<ProductModel> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                //NotifyOfPropertyChange(() => Products);
            }
        }

        private ProductModel _selectedProduct;

        public ProductModel SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        private ObservableCollection<CartItemModel> _cart = new ObservableCollection<CartItemModel>();

        public ObservableCollection<CartItemModel> Cart
        {
            get { return _cart; }
            set
            {
                 _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }

        private int _itemQuantity = 0;
        public int ItemQuantity
        {
            get { return _itemQuantity; }
            set
            {
                _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        public string SubTotal
        {
            get
            {
                return CalculateSubTotal().ToString("C");
            }
        }

        private decimal CalculateTax()
        {
            decimal taxRate = _configHelper.GetTaxRate() / 100;
            decimal taxAmount  = Cart
                .Where(x => x.Product.IsTaxable)
                .Sum(x => x.Product.RetailPrice * x.QuantityInCart * taxRate);

            return taxAmount;
        }

        public string Tax
        {
            get
            {
                return CalculateTax().ToString("C");
            }
        }

        private decimal CalculateSubTotal()
        {
            decimal subTotal = 0;

            foreach (var item in Cart)
            {
                subTotal += (item.Product.RetailPrice * item.QuantityInCart);
            }

            return subTotal;
        }

        public string Total
        {
            get
            {
                decimal total = CalculateSubTotal() + CalculateTax();
                return $"{total:C2}";
            }
        }

        public void AddToCart()
        {

            if (Cart.Any(c => c.Product.Id == SelectedProduct.Id))
            {
                var cartItem = Cart.Where(c => c.Product.Id == SelectedProduct.Id).First();

                Cart.Replace2(cartItem, new CartItemModel
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity + cartItem.QuantityInCart,
                });
            }
            else
            {            
                Cart.Add(new CartItemModel
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity,
                });
            }
                
            SelectedProduct.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1;

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckout);

        }

        public bool CanAddToCart
        {
            get
            {
                bool output = false;

                if (ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity)
                {
                    output = true;
                }

                return output;
            }
        }

        public void RemoveToCart()
        {
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckout);
        }

        public bool CanRemoveToCart
        {
            get
            {
                bool output = false;




                return output;
            }
        }

        public async void Checkout()
        {
            var sale = new SaleModel();

            Cart.ToList().ForEach(cart =>
            {
                sale.SaleDetails.Add(
                    new SaleDetailModel()
                    {
                        ProductId = cart.Product.Id,
                        Quantity = cart.QuantityInCart
                    });
            });

            await _saleEndpoint.PostSale(sale);

          
        }

        public bool CanCheckout
        {
            get
            {
                return Cart.Count > 0;
            }
        }

    }
}
