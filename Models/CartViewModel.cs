namespace IntexBrickwell.Models;

public class CartViewModel
{
    public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    public float TotalPrice => CartItems.Sum(item => item.ItemPrice * item.Quantity);

}
public class CartItem
{
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public string ItemName { get; set; } // This maps to the Name field in your Product model
    public float ItemPrice { get; set; } // This maps to the Price field in your Product model
    public string ImgLink { get; set; } // New field to include the image link from your Product model
    public int Quantity { get; set; } // Maps to the qty field in your LineItem model
}
