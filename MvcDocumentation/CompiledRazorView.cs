namespace Ancestry.Daisy.Documentation.MVC
{
    using System.Web.Mvc;

    internal class CompiledRazorView : ActionResult
    {
        private readonly string viewName;
        private readonly object model;

        public CompiledRazorView(string viewName, object model)
        {
            this.viewName = viewName;
            this.model = model;
        }

        /// <summary>
        /// Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult"/> class.
        /// </summary>
        /// <param name="context">The context in which the result is executed. The context information includes the controller, HTTP content, request context, and route data.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            var rendered = RazorEngine.Razor.Run(viewName, model);
            context.HttpContext.Response.Write(rendered);
        }
    }
}