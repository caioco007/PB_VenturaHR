using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VenturaHR.ViewComponents.ResponseCriteria
{
    public class ResponseCriteriaViewComponent : ViewComponent
    {
        public ResponseCriteriaViewComponent() { }

        public async Task<IViewComponentResult> InvokeAsync(List<DTO.OpportunityCriterion.OpportunityCriterionViewModel> model)
        {
            List<DTO.ResponseCriterion.ResponseCriterionViewModel> responseCriterionViewModelList = new List<DTO.ResponseCriterion.ResponseCriterionViewModel>();
            
            foreach (var item in model)
            {
                var responseCriterionViewModel = new DTO.ResponseCriterion.ResponseCriterionViewModel();
                responseCriterionViewModel.OpportunityCriterionId = item.OpportunityCriterionId;
                responseCriterionViewModel.Criterion = item.Criterion;
                responseCriterionViewModel.Description = item.Description;
                responseCriterionViewModel.Weight = item.Weight;

                responseCriterionViewModelList.Add(responseCriterionViewModel);
            }

            return View(responseCriterionViewModelList);
        }
    }
}
