namespace IntexBrickwell.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;
    using IntexBrickwell.Models;
    using IntexBrickwell.Utilities;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class ProductTypesViewComponent : ViewComponent
    {
        private IProductRepository _repo;
        public ProductTypesViewComponent(IProductRepository temp)
        {
            _repo = temp;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedProductCategory = RouteData?.Values["productCategory"];

            var productCategory = _repo.Products
                .Select(x => x.Category)
                .Distinct()
                .OrderBy(x => x);

            return View(productCategory);
        }
    }
}
