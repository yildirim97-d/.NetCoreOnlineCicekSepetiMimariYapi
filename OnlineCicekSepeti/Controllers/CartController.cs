using Business.Models;
using Business.Services.Bases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineCicekSepeti.Controllers
{
    [Authorize(Roles = "User")]
    public class CartController : Controller
    {

        private readonly IProductService _productService;
        public CartController(IProductService productService)
        {
            _productService = productService;
        }



        public IActionResult AddToCart(int? productId)
        {
            if (productId == null)
                return View("NotFound");
            var product = _productService.GetQuery().SingleOrDefault(p => p.Id == productId.Value);
            var userId = User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid).Value;
            List<CartModel> cart = new List<CartModel>();
            CartModel cartItem;
            string cartJson;

            if (HttpContext.Session.GetString("cart") != null)
            {

                cartJson = HttpContext.Session.GetString("cart");
                cart = JsonConvert.DeserializeObject<List<CartModel>>(cartJson);
            }
            cartItem = new CartModel()
            {
                ProductId = productId.Value,
                ProductName = product.ProductName,
                UnitPrice = product.UnitPrice,
                UserId = Convert.ToInt32(userId),
                ProductImage = product.ImageFileName
                
            };
            cart.Add(cartItem);
            cartJson = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString("cart", cartJson);
            TempData["Message"] = product.ProductName + " Sepete Eklendi!";
            return RedirectToAction("Index","Products"); //1. yöntem
        }

        public IActionResult Index()
        {
            List<CartModel> cart = new List<CartModel>();
            if (HttpContext.Session.GetString("cart") != null)
            {
                cart = JsonConvert.DeserializeObject<List<CartModel>>(HttpContext.Session.GetString("cart"));

            }

            List<CartGroupByModel> cartGroupByModels = (from c in cart
                                                        group c by new { c.ProductId, c.ProductName, c.UserId, c.ProductImage }
                    into cGroupBy
                                                        select new CartGroupByModel()
                                                        {
                                                            ProductId = cGroupBy.Key.ProductId,
                                                            UserId = cGroupBy.Key.UserId,
                                                            ProductName = cGroupBy.Key.ProductName,
                                                            TotalUnitPrice = cGroupBy.Sum(cgb => cgb.UnitPrice).ToString(new CultureInfo("tr")),
                                                            TotalCount = cGroupBy.Count(),
                                                            ProductImage = cGroupBy.Key.ProductImage
                                                            
                                                            

                                                        }).ToList();



            
            return View(cartGroupByModels);
           


        
        }
        public IActionResult AllRemoveCart()
        {
            HttpContext.Session.Remove("cart");
            TempData["CartMessage"] = "Sepetiniz Temizlendi!";
            return RedirectToAction(nameof(Index));

        }

        public IActionResult RemoveCart(int? userId, int? productId)
        {
            if (userId == null || productId == null)
            {
                return View("NotFound");
            }
            CartModel item = null;
            if (HttpContext.Session.GetString("cart") != null)
            {
                List<CartModel> cart = JsonConvert.DeserializeObject<List<CartModel>>(HttpContext.Session.GetString("cart"));
                item = cart.FirstOrDefault(c => c.ProductId == productId.Value && c.UserId == userId.Value);
                if (item != null)
                    cart.Remove(item);
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
            }
            if (item != null)
                TempData["CartMessage"] = item.ProductName + " Ürünü Silindi.";
            return RedirectToAction(nameof(Index));

        }


    }
}

