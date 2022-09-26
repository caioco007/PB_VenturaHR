using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VenturaHR.ViewComponents.ResponseCriterion
{
    public class ResponseCriterionViewComponent : ViewComponent
    {
        public class Wrapper
        {
            public int Index { get; set; }
            public DTO.ResponseCriterion.ResponseCriterionViewModel ResponseCriterionViewModel { get; set; }

            public Wrapper(int index, DTO.ResponseCriterion.ResponseCriterionViewModel responseCriterionViewModel)
            {
                this.Index = index;
                this.ResponseCriterionViewModel = responseCriterionViewModel;
            }
        }

        public ResponseCriterionViewComponent() { }

        public async Task<IViewComponentResult> InvokeAsync(int index, DTO.ResponseCriterion.ResponseCriterionViewModel responseCriterionViewModel) => await Task.FromResult<IViewComponentResult>(View(new Wrapper(index, responseCriterionViewModel)));
    }
}
