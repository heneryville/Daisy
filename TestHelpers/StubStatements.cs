using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry.Daisy.TestHelpers
{
    using Ancestry.Daisy.Statements;

    public class StubStatements<T> : StatementController<T>
    {
        public bool t()
        {
            return true;
        }

        public bool f()
        {
            return true;
        }
    }

    public class StubStatements : StatementController<int>
    {
        public bool odd()
        {
            return Scope % 2 == 1;
        }

        public bool even()
        {
            return !odd();
        }
    }
}
