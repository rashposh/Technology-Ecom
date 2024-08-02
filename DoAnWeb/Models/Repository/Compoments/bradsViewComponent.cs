using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnWeb.Models.Repository.Compoments
{
    public class bradsViewcomponent : ViewComponent
    {
        private readonly DataContext _dataContext;

        public bradsViewcomponent(DataContext context)
        {
            _dataContext = context;
        }
        public async Task<IViewComponentResult> InvokeAsync() => View(await _dataContext.Brands.ToListAsync());
    }
}
