﻿using System;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.Services;
using MyShop.WebUi.Controllers;
using MyShop.WebUi.Tests.Mocks;

namespace MyShop.WebUi.Tests.Controllers
{
    [TestClass]
    public class BasketControllerTest
    {
        [TestMethod]
        public void CanAddBasketItem()
        {
            //setup
            IRepository<Basket> baskets = new MockContext<Basket>();
            IRepository<Product> products = new MockContext<Product>();
            IRepository<Order> orders = new MockContext<Order>();
            IRepository<Customer> customers = new MockContext<Customer>();

            var httpContext = new MockHttpContext();

            IBasketService basketService = new BasketService(products,baskets);
            IOrderService orderService = new OrderService(orders);

            var controller = new BasketController(basketService,orderService,customers);
            controller.ControllerContext = new System.Web.Mvc.
                ControllerContext(httpContext,new System.Web.Routing.RouteData(),controller);
            controller.AddToBasket("1");

            //Act
            //basketService.AddToBasket(httpContext,"1");

            Basket basket = baskets.Collection().FirstOrDefault();

            Assert.IsNotNull(basket);
            Assert.AreEqual(1,basket.BasketItems.Count);
            Assert.AreEqual("1",basket.BasketItems.ToList().FirstOrDefault().ProductId);
        }

        [TestMethod]
        public void canGetSummaryModel()
        {
            IRepository<Basket> baskets = new MockContext<Basket>();
            IRepository<Product> products = new MockContext<Product>();
            IRepository<Order> orders = new MockContext<Order>();
            IRepository<Customer> customers = new MockContext<Customer>();

            products.Insert(new Product(){ Id="1",Price = 10.00m});
            products.Insert(new Product(){ Id= "2",Price = 20.00m });

            Basket basket = new Basket();
            basket.BasketItems.Add(new BasketItem() { ProductId = "1", Quantity=2 });
            basket.BasketItems.Add(new BasketItem() { ProductId = "2", Quantity = 1 });
            baskets.Insert(basket);

            IBasketService basketService = new BasketService(products, baskets);
            IOrderService orderService = new OrderService(orders);

            var controller = new BasketController(basketService, orderService, customers);
            var httpContext = new MockHttpContext();
            httpContext.Request.Cookies.Add(new System.Web.HttpCookie("eCommerceBasket") { Value = basket.Id });
            controller.ControllerContext = new System.Web.Mvc.
                ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);

            var result = controller.BasketSummary() as PartialViewResult;
            var basketSummary = (BasketSummaryViewModel)result.ViewData.Model;

            Assert.AreEqual(3,basketSummary.BasketCount);
            Assert.AreEqual(40.00m,basketSummary.BasketTotal);

        }

        [TestMethod]
        public void canCheckoutAndCreateOrder()
        {
            IRepository<Product> products = new MockContext<Product>();
            products.Insert(new Product() { Id = "1", Price = 10.00m });
            products.Insert(new Product() { Id = "2", Price = 5.00m });

            IRepository<Basket> baskets = new MockContext<Basket>();
            Basket basket = new Basket();
            basket.BasketItems.Add(new BasketItem() { ProductId = "1", Quantity = 2, BasketId = basket.Id });
            basket.BasketItems.Add(new BasketItem() { ProductId = "2", Quantity = 1, BasketId = basket.Id });

            baskets.Insert(basket);

            IBasketService basketService = new BasketService(products , baskets);

            IRepository<Order> orders = new MockContext<Order>();
            IOrderService orderService = new OrderService(orders);
            IRepository<Customer> customers = new MockContext<Customer>();

            customers.Insert(new Customer() { Id = "1", Email = "jjpatel@gmail.com", ZipCode = "380026" });

            IPrincipal FakeUser = new GenericPrincipal(new GenericIdentity("jjpatel@gmail.com","Forms"),null);

            var controller =new  BasketController(basketService,orderService, customers);
            var httpContext = new MockHttpContext();

            httpContext.User = FakeUser;

            httpContext.Request.Cookies.Add(new System.Web.HttpCookie("eCommerceBasket")
            {
                Value = basket.Id
            });

            controller.ControllerContext = new System.Web.Mvc.
                ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);

            //Act
            Order order = new Order();
            controller.Checkout(order);

            //Assert
            Assert.AreEqual(2, order.OrderItems.Count);
            Assert.AreEqual(0, basket.BasketItems.Count);
            
            Order orderInRep = orders.Find(order.Id);
            Assert.AreEqual(2,orderInRep.OrderItems.Count);
            
        }
    }
}
