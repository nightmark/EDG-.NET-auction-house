using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using AuctionHouse.Models;

namespace AuctionHouse.Controllers
{
    public class ProductsController : Controller
    {

        private static IProductsDb productsDB = new IProductsDbImpl();
        
        //
        // GET: /Products/

        public ActionResult Index()
        {
            return View(productsDB.getAllProducts());
        }

        //
        // GET: /Products/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Products/Create

        [HttpPost]
        public ActionResult Create(Product product)
        {
            try
            {
                productsDB.addProduct(product);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View("Error", e);
            }
        }
        
        //
        // GET: /Products/Edit/5
 
        public ActionResult Edit(int id)
        {            
            return View(productsDB.getProduct(id));
        }

        //
        // POST: /Products/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                Product product = new Product(collection.Get("name"), collection.Get("description"), int.Parse(collection.Get("bid")));
                product.id = id;
                productsDB.updateProduct(product);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Products/Bid/5

        public ActionResult Bid(int id) {
            return View(productsDB.getProduct(id));
        }

        //
        // POST: /Products/Bid/5

        [HttpPost]
        public ActionResult Bid(int id, FormCollection collection) {
            try {
                Product product = productsDB.getProduct(id);
                product.bid = int.Parse(collection.Get("bid"));
                productsDB.updateProduct(product);
                return RedirectToAction("Index");
            }
            catch {
                return View();
            }
        }

        //
        // GET: /Products/Delete/5
 
        public ActionResult Delete(int id)
        {
            productsDB.removeProduct(id);
            return RedirectToAction("Index");
        }

    }
}
