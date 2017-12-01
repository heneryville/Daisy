namespace Ancestry.Daisy
{
    using Ancestry.Daisy.Language;
    using Ancestry.Daisy.Linking;
    using Ancestry.Daisy.Program;
    using Ancestry.Daisy.Statements;

    public class DaisyCompiler
    {
        public static DaisyProgram<T> Compile<T>(string code, StatementSet statements, DaisyMode mode = DaisyMode.Debug)
        {
            var ast = DaisyParser.Parse(code);
            var linker = new DaisyLinker(ast, statements, typeof(T));
            linker.Link();
            return new DaisyProgram<T>(ast, mode);
        }
    }

    public enum DaisyMode
    {
        Debug,
        Release
    }
}
