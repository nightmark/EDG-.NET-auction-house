using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace AuctionHouse.Models {
    public class IProductsDbImpl : IProductsDb {
        public const String PRODUCT_IDS_KEY = "productsKeys";

        private RESTcache<String, Object> cache = new RESTcache<String,Object>();
        private int count = -1;

        public IProductsDbImpl()
            : base() {
            List<String> productsList = new List<string>();
            cache.put(PRODUCT_IDS_KEY, productsList);
        }

        public List<String> getAllProductList() {
            return (List<String>)cache.get(PRODUCT_IDS_KEY);            
        }        

        public Product removeProduct(int id) {            
            List<String> productKeys = getAllProductList();
            productKeys.Remove(id.ToString());
            cache.replace(PRODUCT_IDS_KEY, productKeys);
            return (Product)cache.remove(id.ToString());
        }

        public Product getProduct(int id) {
            return (Product)cache.get(id.ToString());
        }

        public Product addProduct(Product product) {
            if (count == -1) {
                count = getAllProductList().Count();
            }
            product.id = count;
            cache.put((product.id).ToString(), product);
            List<String> productsList = getAllProductList();
            if (productsList == null) productsList = new List<String>();
            productsList.Add((product.id).ToString());
            count++;
            return (Product)cache.replace(PRODUCT_IDS_KEY, productsList);
        }

        public Product updateProduct(Product product) {
            return (Product)cache.replace(product.id.ToString(), product);
        }

        public List<Product> getAllProducts() {
            List<Product> products = new List<Product>();
            foreach(String id in getAllProductList()){
                products.Add((Product)cache.get(id));
            }
            return products;
        }
    }
}