﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShop.Core.Models;

namespace MyShop.Core.ViewModels
{
    public class ProductManagerViewModel
    {
        public Product Products { get; set; } 
        public IEnumerable<ProductCategory> ProductCategories { get; set; }
    }
}
