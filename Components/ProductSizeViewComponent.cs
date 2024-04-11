using Microsoft.AspNetCore.Mvc;

namespace IntexBrickwell.Components
{
    public class ProductSizeViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int currentPageSize)
        {
            var pageSizeOptions = new List<int> { 5, 10, 20 }; // Default page sizes
            ViewBag.CurrentPageSize = currentPageSize;
            return View(pageSizeOptions);
        }
    }
}
