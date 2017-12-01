namespace Ancestry.Daisy.Program
{
    using System;

    public class DaisyRuntimeException : Exception
    {
        public DaisyRuntimeException(string message) : base(message){}
    }
}
