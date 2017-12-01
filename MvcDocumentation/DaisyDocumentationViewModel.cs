namespace Ancestry.Daisy.Documentation.MVC
{
    using System.Collections.Generic;

    public class DaisyDocumentationViewModel
    {
        public string Description { get; set; }
        public IList<DaisyStatementDocumentationViewModel> Statements { get; set; }
        public IList<string> ScopeOrder { get; set; }
    }

    public class DaisyStatementDocumentationViewModel
    {
        public string Scope { get; set; }
        public string Title { get; set; }
        public IList<DaisyParameterDocumentationViewModel> Parameters { get; set; }
        public string Description { get; set; }
        public string TransformsTo { get; set; }
    }

    public class DaisyParameterDocumentationViewModel
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
