namespace Ancestry.Daisy.Documentation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.IO;
    using System.Reflection;
    using System.Xml.Linq;

    using Ancestry.Daisy.Documentation.Utils;
    using Ancestry.Daisy.Utils;

    public interface ICommentDocumentation
    {
        MethodDocumentation ForMethod(MethodInfo methodInfo);
    }

    public class CommentDocumentation : ICommentDocumentation
    {
        private Dictionary<string, MethodDocumentation> methodDocs;

        private CommentDocumentation(XDocument document)
        {
            Parse(document);
        }

        public static CommentDocumentation Parse(Stream stream)
        {
            return new CommentDocumentation(XDocument.Load(stream));
        }

        public static CommentDocumentation Parse(string text)
        {
            return new CommentDocumentation(XDocument.Parse(text));
        }

        public static CommentDocumentation ParseFile(string fileName)
        {
            using (var stream  = new FileStream(fileName, FileMode.Open))
            {
                return new CommentDocumentation(XDocument.Load(stream));
            }
        }

        private void Parse(XDocument document)
        {
            methodDocs = document
                .Descendants()
                .Where(x =>
                    x.Name == "member"
                    && x.Attributes().Any(y => y.Name == "name" && y.Value.StartsWith("M:"))
                )
                .Select(x => new MethodDocumentation()
                    {
                        MethodName = x.Attributes()
                            .First(y => y.Name == "name")
                            .Value
                            .Substring(2),
                        Summary = x.Descendants()
                            .FirstOrDefault(y => y.Name == "summary")
                            .With(AllInclusiveValue)
                            .With(y => y.ConsolidateWhitespace()),
                        Parameters = x.Descendants()
                                      .Where(y => y.Name == "param")
                                      .Select(e => new {
                                          name = e.Attributes()
                                            .FirstOrDefault(a => a.Name == "name")
                                            .With(a => a.Value),
                                          description = e.Value
                                      })
                                      .Where(a => !string.IsNullOrEmpty(a.name))
                                      .ToDictionary(z => z.name, z => z.description)
                    })
                .ToDictionary(x => x.MethodName, x => x);
        }

        private string AllInclusiveValue(XElement ele)
        {
            bool ignoreNext = false;
            return string.Join(" ", ele.DescendantNodes().Select(x =>
            {
                if (ignoreNext)
                {
                    ignoreNext = false;
                    return null;
                }
                if (x is XText) return ((XText) x).Value;
                if (x is XElement)
                {
                    var xele = (XElement) x;
                    if (xele.IsEmpty) return "<" + xele.Name + "/>";
                    ignoreNext = true;
                    return "<" + xele.Name + ">" + AllInclusiveValue(xele) + "</" + xele.Name + ">";
                }
                return null;
            }).Where(x => x != null));
        }

        public MethodDocumentation ForMethod(MethodInfo methodInfo)
        {
            return methodDocs.With(methodInfo.GetDocStyleSignature());
        }
    }

    public class MethodDocumentation
    {
        public string MethodName { get; set; }
        public string Summary { get; set; }
        public IDictionary<string,string> Parameters { get; set; }
    }
}
