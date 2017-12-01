namespace Ancestry.Daisy.Documentation
{
    using System.Reflection;

    using Ancestry.Daisy.Statements;

    public static class DaisyDocumentor
    {
        public static DocumentationSet Document(Assembly assembly, string documentationPath)
        {
            return new DocumentationSet(new StatementSet().FromAssembly(assembly), CommentDocumentation.ParseFile(documentationPath));
        }
    }
}
