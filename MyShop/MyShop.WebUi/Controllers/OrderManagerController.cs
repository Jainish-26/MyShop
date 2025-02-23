﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Contracts;
using MyShop.Core.Models;

namespace MyShop.WebUi.Controllers
{
    public class OrderManagerController : Controller
    {
        IOrderService orderService;

        public OrderManagerController(IOrderService orderService)
        {
            this.orderService = orderService;
        }
        // GET: AdminManager
        public ActionResult Index()
        {
            List<Order> orders = orderService.GetOrderList();
            return View(orders);
        }

        public ActionResult UpdateOrder(string Id)
        {
            ViewBag.StatusList = new List<string>()
            {
                "Order Craeted",
                "Payment Proceed",
                "Order Shipped",
                "Order Completed"
            };
            Order order = orderService.GetOrder(Id);
            return View(order); 
        }

        [HttpPost]
        public ActionResult UpdateOrder(Order updateOrder,string Id)
        {
            Order order = orderService.GetOrder(Id);
            order.OrderStatus = updateOrder.OrderStatus;
            orderService.UpdateOrder(order);
            return RedirectToAction("Index");
        }
    }
}