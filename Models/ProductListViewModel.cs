namespace IntexBrickwell.Models
{
    public class ProductListViewModel
    {
        public IQueryable<Product> Products {  get; set; }

        public PaginationInfo PaginationInfo { get; set; } = new PaginationInfo();

        public string? CurrentProductColor { get; set; }
        public string? CurrentProductCategory { get; set; }
    }
}
