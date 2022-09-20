using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VenturaHR.Helpers
{
    public class ViewEngineHelper
    {
        private readonly ICompositeViewEngine viewEngine;

        public ViewEngineHelper(ICompositeViewEngine viewEngine)
        {
            this.viewEngine = viewEngine;
        }

        public async Task<string> RenderPartialViewToString(ControllerContext controllerContext, ViewDataDictionary viewData, ITempDataDictionary tempData, string viewName, object model)
        {
            viewData.Model = model;
            using (var writer = new System.IO.StringWriter())
            {
                ViewEngineResult viewResult;

                if (viewName.EndsWith(".cshtml"))
                    viewResult = viewEngine.GetView(viewName, viewName, false);
                else
                    viewResult = viewEngine.FindView(controllerContext, viewName, false);

                ViewContext viewContext = new ViewContext(controllerContext, viewResult.View, viewData, tempData, writer, new HtmlHelperOptions());
                await viewResult.View.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
        }
    }
}
