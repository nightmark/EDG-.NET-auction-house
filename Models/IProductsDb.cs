using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuctionHouse.Models {
    interface IProductsDb {
        List<Product> getAllProducts();
        Product removeProduct(int id);
        Product getProduct(int id);
        Product addProduct(Product product);
        Product updateProduct(Product product);        
    }
}