/*
	Copyright © Bryan Apellanes 2015  
*/

namespace Bam.Console
{
    /// <summary>
    /// Defines a contract for attributes that provide descriptive information text.
    /// </summary>
    public interface IInfoAttribute
    {
        /// <summary>
        /// Gets or sets the descriptive information text.
        /// </summary>
        string Information { get; set; }
    }
}
