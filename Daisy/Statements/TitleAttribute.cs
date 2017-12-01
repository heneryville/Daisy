using System;

namespace Ancestry.Daisy.Statements
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TitleAttribute : Attribute
    {
        public string Title { get; set; }

        public TitleAttribute(string title)
        {
            Title = title;
        }
    }
}
