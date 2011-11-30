using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuctionHouse.Models {    
    [Serializable]
    public class Product {
        public int bid { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }

        public Product() { }
        public Product(String name, String description, int bid) {
            this.id = -1;
            this.bid = bid;
            this.name = name;
            this.description = description;
        }

    }

}