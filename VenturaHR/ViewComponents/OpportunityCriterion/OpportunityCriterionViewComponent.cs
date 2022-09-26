using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VenturaHR.ViewComponents.OpportunityCriterion
{
    public class OpportunityCriterionViewComponent : ViewComponent
    {
        public class Wrapper
        {
            public int Index { get; set; }
            public DTO.OpportunityCriterion.OpportunityCriterionViewModel OpportunityCriterionViewModel { get; set; }

            public Wrapper(int index, DTO.OpportunityCriterion.OpportunityCriterionViewModel opportunityCriterionViewModel)
            {
                this.Index = index;
                this.OpportunityCriterionViewModel = opportunityCriterionViewModel;
            }
        }

        public OpportunityCriterionViewComponent() { }

        public async Task<IViewComponentResult> InvokeAsync(int index, DTO.OpportunityCriterion.OpportunityCriterionViewModel opportunityCriterionViewModel) => await Task.FromResult<IViewComponentResult>(View(new Wrapper(index, opportunityCriterionViewModel)));
    }
}
