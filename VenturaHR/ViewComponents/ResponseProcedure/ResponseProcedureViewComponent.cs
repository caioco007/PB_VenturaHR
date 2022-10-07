using Microsoft.AspNetCore.Mvc;
using Services.Response;
using System.Threading.Tasks;

namespace VenturaHR.ViewComponents.ResponseProcedure
{
    public class ResponseProcedureViewComponent : ViewComponent
    {
        readonly ResponseService responseService;

        public ResponseProcedureViewComponent(ResponseService responseService)
        {
            this.responseService = responseService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int opportunityId) => await Task.Run(async () => View(await responseService.GetCandidateForOpportunity(opportunityId)));
    }
}
