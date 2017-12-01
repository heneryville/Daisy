namespace Ancestry.Daisy.Language
{
    using Ancestry.Daisy.Language.AST;

    public class DaisyAst
    {
        public DaisyAst(IDaisyAstNode root)
        {
            Root = root;
        }

        public IDaisyAstNode Root { get; private set; }

    }
}
