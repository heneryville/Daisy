namespace Ancestry.Daisy.Tests.TestObjects
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Parent object class
    /// </summary>
    public class ParentObject
    {
        /// <summary>
        /// Gets or sets the lower object.
        /// </summary>
        /// <value>
        /// The lower object.
        /// </value>
        public LowerObject LowerObject { get; set; }

        /// <summary>
        /// Gets or sets the propety1.
        /// </summary>
        /// <value>
        /// The propety1.
        /// </value>
        public int Propety1 { get; set; }

        /// <summary>
        /// Gets or sets the property2.
        /// </summary>
        /// <value>
        /// The property2.
        /// </value>
        public string Property2 { get; set; }

        /// <summary>
        /// Gets or sets the property3.
        /// </summary>
        /// <value>
        /// The property3.
        /// </value>
        public IEnumerable<object> Property3 { get; set; }
    }

    /// <summary>
    /// Lower object
    /// </summary>
    public class LowerObject
    {
        /// <summary>
        /// Gets or sets the date time.
        /// </summary>
        /// <value>
        /// The date time.
        /// </value>
        public DateTime DateTime { get; set; }
    }
}
