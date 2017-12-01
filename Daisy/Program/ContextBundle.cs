using System.Collections.Generic;

namespace Ancestry.Daisy.Program
{
    public class ContextBundle : Dictionary<string,object>
    {
        public T Get<T>(string key)
        {
            if (!this.ContainsKey(key)) return default(T);
            return (T)this[key];
        }
    }
}
