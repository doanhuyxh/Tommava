using Microsoft.AspNetCore.Mvc;
using Tommava.Services;

namespace Tommava.ViewComponents
{
    [ViewComponent(Name = "Category")]
    public class MenuComponent : ViewComponent
    {
        private ICommon _icommom;

        public MenuComponent(ICommon icommom)
        {
            _icommom = icommom;
        }
        public IViewComponentResult Invoke()
        {
            var menu = _icommom.GetAllCategory().OrderByDescending(x => x.CreatedDate);
            return View(menu);
        }
    }
}
