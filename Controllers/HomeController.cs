using System.Diagnostics;
using IntexBrickwell.Data;
using Microsoft.AspNetCore.Mvc;
using IntexBrickwell.Models;
using IntexBrickwell.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
    

namespace IntexBrickwell.Controllers;

public class HomeController : Controller
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ILineItemRepository _lineItemRepository;

    public HomeController(
        IOrderRepository orderRepository, 
        IProductRepository productRepository, 
        IUserRepository userRepository,
        ICustomerRepository customerRepository,
        ILineItemRepository lineItemRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _userRepository = userRepository;
        _customerRepository = customerRepository;
        _lineItemRepository = lineItemRepository;
    }
    public IActionResult Index(int page = 1, int pageSize = 4)
    {
        var totalItems = _productRepository.Products.Count();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var products = _productRepository.Products
            .OrderBy(i => i.ProductId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var model = new HomePageViewModel()
        {
            Products = products,
            CurrentPage = page,
            TotalPages = totalPages
        };

        // Retrieve the cart to calculate item count
        var cart = HttpContext.Session.GetObjectFromJson<CartViewModel>("Cart") ?? new CartViewModel { CartItems = new List<CartItem>() };
        ViewBag.CartItemCount = cart.CartItems.Sum(item => item.Quantity);

        return View(model);
    }
    
    public IActionResult AddToCart(int productID)
    {
        var product = _productRepository.Products.FirstOrDefault(i => i.ProductId == productID);
        var cart = HttpContext.Session.GetObjectFromJson<CartViewModel>("Cart") ?? new CartViewModel { CartItems = new List<CartItem>() };

        var cartItem = cart.CartItems.FirstOrDefault(i => i.ProductId == productID);
        if (cartItem != null)
        {
            cartItem.Quantity++;
        }
        else
        {
            cart.CartItems.Add(new CartItem
            {
                ProductId = product.ProductId,
                ItemName = product.Name,
                ItemPrice = (float)product.Price,
                ImgLink = product.ImgLink,
                Quantity = 1
            });
        }

        HttpContext.Session.SetObjectAsJson("Cart", cart);

        // For AJAX: Return JSON including the updated cart item count
        return Json(new { cartItemCount = cart.CartItems.Sum(c => c.Quantity) });
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CheckoutCart()
    {
        var cart = HttpContext.Session.GetObjectFromJson<CartViewModel>("Cart") ?? new CartViewModel { CartItems = new List<CartItem>() };
        ViewBag.CartItemCount = cart.CartItems.Sum(item => item.Quantity);
        return View(cart);
    }

    public IActionResult Privacy()
    {
        return View();
    }
}