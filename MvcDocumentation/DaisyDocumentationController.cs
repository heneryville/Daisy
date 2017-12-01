namespace Ancestry.Daisy.Documentation.MVC
{
    using System.Collections.Generic;
    using System.IO;
    using System.Web.Mvc;

    using Ancestry.Daisy.Statements;

    using RazorEngine.Templating;

    public class DaisyDocumentationController : Controller
    {
        public ActionResult Index()
        {
            var model = (DaisyDocumentationViewModel)this.RouteData.DataTokens["docSet"];
            return new CompiledRazorView("Ancestry.Daisy.Documentation.MVC.Views.DaisyDocumentation.Index.cshtml", model);
        }
    }
}
