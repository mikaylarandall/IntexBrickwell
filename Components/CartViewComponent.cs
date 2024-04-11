namespace IntexBrickwell.ViewComponents;

using Microsoft.AspNetCore.Mvc;
using IntexBrickwell.Models;
using IntexBrickwell.Utilities;

public class CartViewComponent : ViewComponent
{
    // Optionally inject any services you need, like a cart service or DB context
    public CartViewComponent()
    {
    }

    public IViewComponentResult Invoke()
    {
        // Retrieve cart count. Here you'll add your logic to get the cart count
        int cartItemCount = GetCartItemCount();

        return View(cartItemCount);
    }

    private int GetCartItemCount()
    {
        // Your logic to fetch cart item count, possibly from session or a database
        // For example:
        var cart = HttpContext.Session.GetObjectFromJson<CartViewModel>("Cart");
        return cart?.CartItems.Count ?? 0;
        return 0; // Placeholder return value
    }
}
