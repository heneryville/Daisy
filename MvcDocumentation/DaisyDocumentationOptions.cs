using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.Documentation.MVC
{
    public class DaisyDocumentationOptions
    {
        public DaisyDocumentationOptions()
        {
            ScopeOrder = new List<Type>();
        }

        public IList<Type> ScopeOrder { get; set; }
    }
}
