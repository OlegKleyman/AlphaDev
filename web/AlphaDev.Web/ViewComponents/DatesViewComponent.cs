namespace AlphaDev.Web.ViewComponents
{
    using System;
    using System.Threading.Tasks;

    using AlphaDev.Web.Models;

    using Microsoft.AspNetCore.Mvc;

    using Optional;

    public class DatesViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(DateTime created, Option<DateTime> modified) => Task.Run(
            () => (IViewComponentResult)View("Dates", new DatesViewModel(created, modified)));
    }
}