using Microsoft.AspNetCore.Mvc;
using Services.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VenturaHR.ViewComponents.Shared
{
    [ViewComponent(Name = "Shared")]
    public class AddressViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(DTO.Shared.AddressViewModel model) => await Task.Run(() => { model ??= new DTO.Shared.AddressViewModel(); return View("Address", model); });
    }
}
