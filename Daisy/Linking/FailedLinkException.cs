namespace Ancestry.Daisy.Linking
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class FailedLinkException : Exception
    {
        public IList<LinkingError> Errors { get; set; }

        public FailedLinkException(IList<LinkingError> errors) :base(MakeMessage(errors))
        {
            Errors = errors;
        }

        private static string MakeMessage(IEnumerable<LinkingError> errors)
        {
            var sb = new StringBuilder();
            sb.Append("Linking failed.");
            foreach (var linkingError in errors)
            {
                sb.AppendLine();
                sb.Append(linkingError.ToString());
            }
            return sb.ToString();
        }
    }
}