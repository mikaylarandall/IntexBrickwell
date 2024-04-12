using System.Diagnostics;
using IntexBrickwell.Data;
using Microsoft.AspNetCore.Mvc;
using IntexBrickwell.Models;
using IntexBrickwell.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.AspNetCore.Authorization;


namespace IntexBrickwell.Controllers;

public class HomeController : Controller
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ILineItemRepository _lineItemRepository;
    private readonly InferenceSession _session;
    private readonly ApplicationDbContext _context;
    private readonly IRecommendationRepository _recommendationRepository;
    private readonly ICustomerRecommendationRepository _customerRecommendationRepository;

    public HomeController(
        IOrderRepository orderRepository, 
        IProductRepository productRepository, 
        IUserRepository userRepository,
        ICustomerRepository customerRepository,
        ILineItemRepository lineItemRepository,
        ApplicationDbContext context,
        IRecommendationRepository recommendationRepository,
        ICustomerRecommendationRepository customerRecommendationRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _userRepository = userRepository;
        _customerRepository = customerRepository;
        _lineItemRepository = lineItemRepository;
        _context = context;
        _recommendationRepository = recommendationRepository;
        _customerRecommendationRepository = customerRecommendationRepository;

        try
        {

            // _session = new InferenceSession("/Users/brysonlindsey/Documents/GitHub/IntexBrickwell/decision_tree_model.onnx");
             _session = new InferenceSession("C:\\Users\\carolineconley\\Source\\Repos\\IntexBrickwell\\decision_tree_model.onnx");
            
            //_session = new InferenceSession("C:\\Users\\mikaylarandall\\source\\repos\\IntexBrickwell\\decision_tree_model.onnx");
            // _session = new InferenceSession("C:\\Users\\Tiffany\\source\\repos\\IntexBrickwell\\decision_tree_model.onnx");
            
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    [HttpGet]
    public IActionResult Predict(int customer_ID, int time, int amount, int day_of_week_Mon, int day_of_week_Sat,
        int day_of_week_Sun, int day_of_week_Thu, int day_of_week_Tue, int day_of_week_Wed, int entry_mode_PIN,
        int entry_mode_Tap, int type_of_transaction_Online, int type_of_transaction_POS,
        int country_of_transaction_India, int country_of_transaction_Russia, int country_of_transaction_USA,
        int country_of_transaction_UnitedKingdom, int shipping_address_India, int shipping_address_Russia,
        int shipping_address_USA, int shipping_address_UnitedKingdom, int bank_HSBC, int bank_Halifax, int bank_Lloyds,
        int bank_Metro, int bank_Monzo, int bank_RBS, int type_of_card_Visa)
    {
        var class_type_dict = new Dictionary<int, string>
        {
            {0, "Not Fraud"},
            {1, "Fraud"}
        };

        try
        {
            var input = new List<float> { customer_ID, time, amount, day_of_week_Mon, day_of_week_Sat, day_of_week_Sun, day_of_week_Thu, day_of_week_Tue, day_of_week_Wed, entry_mode_PIN, entry_mode_Tap, type_of_transaction_Online, type_of_transaction_POS, country_of_transaction_India, country_of_transaction_Russia, country_of_transaction_USA, country_of_transaction_UnitedKingdom, shipping_address_India, shipping_address_Russia, shipping_address_USA, shipping_address_UnitedKingdom, bank_HSBC, bank_Halifax, bank_Lloyds, bank_Metro, bank_Monzo, bank_RBS, type_of_card_Visa};
            var inputTensor = new DenseTensor<float>(input.ToArray(), new[] { 1, input.Count });
            
            var inputs = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("float_input", inputTensor)
            };
            
            using var results = _session.Run(inputs);
            {
                var prediction = results.FirstOrDefault(item => item.Name == "output_label")?.AsTensor<long>()
                    .ToArray();
                if (prediction != null && prediction.Length > 0)
                {
                    var order_type = class_type_dict.GetValueOrDefault((int)prediction[0], "Unknown");
                    ViewBag.Prediction = order_type;
                }
                else
                {
                    ViewBag.Prediction = "Error: Unable to make prediction.";
                
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            ViewBag.Prediction = "Error: Unable to make prediction.";
            throw;
        }

        return View("Index");

    }
    
    public IActionResult ReviewOrders()
    {
        var records = _context.Orders.ToList();
        var predictions = new List<FraudPrediction>();
        
        var class_type_dict = new Dictionary<int, string>
        {
            {0, "Not Fraud"},
            {1, "Fraud"}
        };

        foreach (var record in records.Take(100))
        {
            
            float time = record.Time ?? 20;
            float amount = record.Amount ?? 100;
            var input = new List<float>
            {
                record.CustomerId, time, amount, record.DayOfWeek == "Mon" ? 1 : 0,
                record.DayOfWeek == "Sat" ? 1 : 0, record.DayOfWeek == "Sun" ? 1 : 0, record.DayOfWeek == "Thu" ? 1 : 0,
                record.DayOfWeek == "Tue" ? 1 : 0, record.DayOfWeek == "Wed" ? 1 : 0, record.EntryMode == "PIN" ? 1 : 0,
                record.EntryMode == "Tap" ? 1 : 0, record.TypeOfTransaction == "Online" ? 1 : 0,
                record.TypeOfTransaction == "POS" ? 1 : 0, record.CountryOfTransaction == "India" ? 1 : 0,
                record.CountryOfTransaction == "Russia" ? 1 : 0, record.CountryOfTransaction == "USA" ? 1 : 0,
                record.CountryOfTransaction == "United Kingdom" ? 1 : 0, record.ShippingAddress == "India" ? 1 : 0,
                record.ShippingAddress == "Russia" ? 1 : 0, record.ShippingAddress == "USA" ? 1 : 0,
                record.ShippingAddress == "United Kingdom" ? 1 : 0, record.Bank == "HSBC" ? 1 : 0,
                record.Bank == "Halifax" ? 1 : 0, record.Bank == "Lloyds" ? 1 : 0, record.Bank == "Metro" ? 1 : 0,
                record.Bank == "Monzo" ? 1 : 0, record.Bank == "RBS" ? 1 : 0, record.TypeOfCard == "Visa" ? 1 : 0
            };
            var inputTensor = new DenseTensor<float>(input.ToArray(), new[] { 1, input.Count });

            var inputs = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("float_input", inputTensor)
            };

            string predictionResult;
            using var results = _session.Run(inputs);
            {
                var prediction = results.FirstOrDefault(item => item.Name == "output_label")?.AsTensor<long>()
                    .ToArray();
                predictionResult = prediction != null && prediction.Length > 0
                    ? class_type_dict.GetValueOrDefault((int)prediction[0], "Unknown")
                    : "Error: Unable to make prediction.";
            }

            predictions.Add(new FraudPrediction { Order = record, Prediction = predictionResult });
        }

        return View(predictions);
    }


    //public IActionResult Products(int pageNum, string? productCategory, string? productColor)
    //{
    //    int pageSize = 20; // Sets the number of products per page

    //    // Apply filters directly in the LINQ query
    //    var products = _productRepository.Products
    //        .Where(x =>
    //            (string.IsNullOrEmpty(productCategory) || x.Category == productCategory) &&
    //            (string.IsNullOrEmpty(productColor) || x.PrimaryColor == productColor))
    //        .OrderBy(x => x.Name)
    //        .Skip((pageNum - 1) * pageSize)
    //        .Take(pageSize)
    //        .ToList();

    //    // Calculate the total number of items after applying filters
    //    int totalItems = _productRepository.Products
    //        .Count(x =>
    //            (string.IsNullOrEmpty(productCategory) || x.Category == productCategory) &&
    //            (string.IsNullOrEmpty(productColor) || x.PrimaryColor == productColor));

    //    var productData = new ProductListViewModel
    //    {
    //        Products = products,
    //        PaginationInfo = new PaginationInfo
    //        {
    //            CurrentPage = pageNum,
    //            ItemsPerPage = pageSize,
    //            TotalItems = totalItems
    //        },
    //        CurrentProductCategory = productCategory,
    //        CurrentProductColor = productColor // Assuming your ViewModel supports this property
    //    };

    //    return View(productData);
    //}

    public IActionResult Products(int pageNum, string? productCategory, string? productColor, int pageSize = 20)
    {
        var products = _productRepository.Products
            .Where(x =>
                (string.IsNullOrEmpty(productCategory) || x.Category == productCategory) &&
                (string.IsNullOrEmpty(productColor) || x.PrimaryColor == productColor))
            .OrderBy(x => x.Name)
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        int totalItems = _productRepository.Products
            .Count(x =>
                (string.IsNullOrEmpty(productCategory) || x.Category == productCategory) &&
                (string.IsNullOrEmpty(productColor) || x.PrimaryColor == productColor));

        var productData = new ProductListViewModel
        {
            Products = products,
            PaginationInfo = new PaginationInfo
            {
                CurrentPage = pageNum,
                ItemsPerPage = pageSize,
                TotalItems = totalItems
            },
            CurrentProductCategory = productCategory,
            CurrentProductColor = productColor
        };

        return View(productData);
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
    public IActionResult Aboutus()
    {
        return View();
    }

    public IActionResult AdminProducts()
    {
        var all = _productRepository.Products
            .OrderBy(x => x.Name);

        return View("AdminProducts", all);
    }

    [HttpGet]
    public IActionResult AddProduct()
    {
        return View(new Product());
    }

    [Authorize(Roles = "Customer")]
    [HttpGet]
    public IActionResult CheckoutForm()
    {
        return View(new Order());
    }

    [HttpPost]
    public IActionResult CheckoutForm(Order o)
    {
        if (ModelState.IsValid)
        {
            _orderRepository.AddOrder(o);
        }

        // If model state is not valid, return to the form with the current model to show validation errors.
        return View(o);
    }

    //[HttpPost]
    //public IActionResult AddProduct(Product p)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        _productRepository.AddProduct(p);
    //    }

    //    return RedirectToAction("AdminProducts", new Product());

    //}

    [HttpPost]
    public IActionResult AddProduct(Product p)
    {
        if (ModelState.IsValid)
        {
            _productRepository.AddProduct(p);
            return RedirectToAction("AdminProducts"); // Assuming "AdminProducts" is a view listing all products.
        }

        // If model state is not valid, return to the form with the current model to show validation errors.
        return View(p);
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var record = _productRepository.Products
             .Single(x => x.ProductId == id);

        return View("AddProduct", record);
    }

    [HttpPost]
    public IActionResult Edit(Product product)
    {
        if (ModelState.IsValid)
        {
            _productRepository.EditProduct(product);

        }
        return RedirectToAction("AdminProducts");
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var recordToDelete = _productRepository.Products
            .Single(x => x.ProductId == id);

        return View(recordToDelete);
    }

    [HttpPost]
    public IActionResult Delete(Product product)
    {
        _productRepository.DeleteProduct(product);

        return RedirectToAction("AdminProducts");
    }
    public IActionResult CustomerRecommendation()
    {
        var all = _customerRecommendationRepository.CustomerRecommendations
            .OrderBy(x => x.Customer_ID);

        return View("CustomerRecommendation", all);
    }
    
    public IActionResult ProductRecommendation()
    {
        var all = _recommendationRepository.ProductRecommendation
            .OrderBy(x => x.ProductID);

        return View("ProductRecommendation", all);
    }

    
    public IActionResult Productdetails(int productId)
    {
        var product = _productRepository.Products.FirstOrDefault(p => p.ProductId == productId);

        if (product == null)
        {
            return NotFound(); // or redirect to another view if you prefer
        }

        return View(product); // Pass the product to the view
    }
}