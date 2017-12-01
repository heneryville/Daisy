using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Documentation.MVC
{
    using System.IO;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Ancestry.Daisy.Statements;

    public static class DocumentationSetExtentions
    {
        private static bool isInitialized = false;

        internal static void Init()
        {
            if (isInitialized) return;
            Compile("Ancestry.Daisy.Documentation.MVC.Views.DaisyDocumentation.Index.cshtml");
            isInitialized = true;
        }

        private static void Compile(string resource)
        {
            var assembly = typeof(DocumentationSetExtentions).Assembly;
            var view = assembly.GetManifestResourceStream(resource);
            RazorEngine.Razor.Compile(new StreamReader(view).ReadToEnd(),resource);
        }

        public static void ExposeWebDocs(this DocumentationSet docs, RouteCollection routes, string setName, DaisyDocumentationOptions options = null)
        {
            options = options ?? new DaisyDocumentationOptions();
            Init();
            routes.MapRoute("Daisy Documentation " + setName,
                url: "daisy/docs/" + setName,
                defaults: new { controller = "DaisyDocumentation", action = "Index" },
                namespaces:new []{"Ancestry.Daisy.Documentation"})
                .DataTokens["docSet"] = toView(docs,setName, options);
        }

        internal static DaisyDocumentationViewModel toView(DocumentationSet docs, string setName, DaisyDocumentationOptions options)
        {
            return new DaisyDocumentationViewModel()
            {
                Description = setName,
                Statements = docs.Statements
                  .OrderBy(x => x.Description)
                  .Select(stat => new DaisyStatementDocumentationViewModel()
                  {
                      Scope = stat.ScopeType.Name,
                      Description = stat.Description,
                      Title = stat.Title,
                      TransformsTo = stat.Parameters
                                    .Where(y => y.TransformsTo != null)
                                    .Select(y => y.TransformsTo.Name)
                                    .FirstOrDefault(),
                      Parameters = stat.Parameters.Where(p => p.TransformsTo == null)
                          .Select(p => new DaisyParameterDocumentationViewModel() {
                              Name = p.Name,
                              Type = p.Type.Name,
                              Description = p.Description
                          })
                          .ToList()
                  })
                  .ToList(),
                  ScopeOrder = options.ScopeOrder.Select(x => x.Name).ToList()
            };
        }
    }
}
