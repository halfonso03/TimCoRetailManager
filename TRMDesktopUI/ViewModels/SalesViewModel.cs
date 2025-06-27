using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TRMDataManager.Library.Models;
using TRMDesktopUI.Library.Api;

namespace TRMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private readonly IProductEndpoint _productEndpoint;


        protected override async void OnViewLoaded(object view)
        {

            base.OnViewLoaded(view);
            var p = await _productEndpoint.GetAll();
            Products = new BindingList<ProductModel>(p);
        }

        public SalesViewModel(IProductEndpoint productEndpoint)
        {
            _productEndpoint = productEndpoint;
        }

        private BindingList<ProductModel> _products;

        public BindingList<ProductModel> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        private BindingList<string> _cart;

        public BindingList<string> Cart
        {
            get { return _cart; }
            set
            {
                 _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }

        private int _itemQuantity;
        

        public int ItemQuantity
        {
            get { return _itemQuantity; }
            set
            {
                _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
            }
        }

        public string SubTotal
        {
            get
            {
                return "$0.00";
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

        }

        public bool CanAddToCart
        {
            get
            {
                bool output = false;




                return output;
            }
        }

        public void RemoveToCart()
        {

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
