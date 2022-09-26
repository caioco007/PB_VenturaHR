using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VenturaHR.ViewComponents.OpportunityCriteria
{
    public class OpportunityCriteriaViewComponent : ViewComponent
    {
        public OpportunityCriteriaViewComponent() { }

        public async Task<IViewComponentResult> InvokeAsync(List<DTO.OpportunityCriterion.OpportunityCriterionViewModel> model)
        {
            return View(model);
        }
    }
}
