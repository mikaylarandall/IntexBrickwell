using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using IntexBrickwell.Models;

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
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
}