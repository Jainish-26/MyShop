﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.ViewModels
{
    public class BasketItemViewModel
    {
        public string Id { get; set; }  
        public int Quantity { get; set; }
        public string Productname { get; set; }
        public decimal Price {  get; set; }
        public string Image { get; set;  }
    }
}
