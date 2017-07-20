using System;
using System.Threading.Tasks;
using AlphaDev.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Optional;

namespace AlphaDev.Web.ViewComponents
{
    public class DatesViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(DateTime created, Option<DateTime> modified)
        {
            return Task.Run(
                () => (IViewComponentResult) View("Dates", new DatesViewModel(created, modified)));
        }
    }
}