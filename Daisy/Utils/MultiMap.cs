using System.Collections.Generic;
using System.Linq;

namespace Ancestry.Daisy.Utils
{
    public class MultiMap<TK,TV>
    {

        private Dictionary<TK, IList<TV>> dict = new Dictionary<TK, IList<TV>>();

        public IEnumerable<TV> this[TK key]
        {
            get
            {
                return dict.ContainsKey(key) ? dict[key]  : Enumerable.Empty<TV>();
            }
        }

        public void Add(TK key, TV value)
        {
            var list = dict.ContainsKey(key) ? dict[key] : null;
            if(list == null)
            {
                list = new List<TV>();
                dict.Add(key, list);
            }
            list.Add(value);
        }

    }
}
