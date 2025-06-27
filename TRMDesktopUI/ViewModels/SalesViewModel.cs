using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TRMDataManager.Library.Models;
using TRMDesktopUI.Library.Api;
using TRMDesktopUI.Library.Helpers;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private readonly IProductEndpoint _productEndpoint;


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

        public SalesViewModel(IProductEndpoint productEndpoint)
        {
            _productEndpoint = productEndpoint;
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

                decimal subTotal = 0;

                foreach(var c in Cart)
                {
                    subTotal += c.QuantityInCart * c.Product.RetailPrice;
                }

                return $"{subTotal:C2}";
            }
        }

        public string Tax
        {
            get
            {
                return "$0.00";
            }
        }

        public string Total
        {
            get
            {
                return "$0.00";
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
                var cartItem = new CartItemModel
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity,
                };

                Cart.Add(cartItem);
            }
                
            SelectedProduct.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1;

            NotifyOfPropertyChange(() => SubTotal);
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
        }

        public bool CanRemoveToCart
        {
            get
            {
                bool output = false;




                return output;
            }
        }

        public void Checkout()
        {

        }

        public bool CanCheckout
        {
            get
            {
                bool output = false;




                return output;
            }
        }

    }
}
