using IntexBrickwell.Models;
using Microsoft.AspNetCore.Mvc;

namespace IntexBrickwell.Components
{
    public class ProductColorsViewComponent : ViewComponent
    {
        private IProductRepository _repo;
        public ProductColorsViewComponent(IProductRepository temp)
        {
            _repo = temp;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedProductColor = RouteData?.Values["productColor"];

            var productCategory = _repo.Products
                .Select(x => x.PrimaryColor)
                .Distinct()
                .OrderBy(x => x);

            return View(productCategory);
        }
    }
}
