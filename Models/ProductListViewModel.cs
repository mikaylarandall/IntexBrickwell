using Microsoft.AspNetCore.Mvc.Rendering;

namespace IntexBrickwell.Models
{
    public class ProductListViewModel
    {
        public IEnumerable<Product> Products {  get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public PaginationInfo PaginationInfo { get; set; }
        public string? CurrentProductColor { get; set; }
        public string? CurrentProductCategory { get; set; }
    }
}
