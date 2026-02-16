/*
	Copyright © Bryan Apellanes 2015  
*/

namespace Bam.Console
{
    /// <summary>
    /// Provides a dictionary-based lookup of <see cref="ArgumentInfo"/> instances by argument name.
    /// </summary>
    public class ArgumentInfoHash
    {
        readonly Dictionary<string, ArgumentInfo> _innerHash;
        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentInfoHash"/> class from an array of <see cref="ArgumentInfo"/> entries.
        /// </summary>
        /// <param name="argumentInfo">The argument info entries to index by name.</param>
        public ArgumentInfoHash(ArgumentInfo[] argumentInfo)
        {
            _innerHash = new Dictionary<string, ArgumentInfo>(argumentInfo.Length);
            foreach (ArgumentInfo info in argumentInfo)
            {
                if (_innerHash.ContainsKey(info.Name))
                    _innerHash[info.Name] = info;
                else
                    _innerHash.Add(info.Name, info);
            }
        }
        /// <summary>
        /// Gets all argument names stored in this hash.
        /// </summary>
        public string[] ArgumentNames
        {
            get
            {
                List<string> returnValue = new List<string>(_innerHash.Keys.Count);
                foreach (string name in _innerHash.Keys)
                {
                    returnValue.Add(name);
                }
                return returnValue.ToArray();
            }
        }

        /// <summary>
        /// Gets the <see cref="ArgumentInfo"/> for the specified argument name, or null if not found.
        /// </summary>
        /// <param name="argumentName">The name of the argument to look up.</param>
        /// <returns>The matching <see cref="ArgumentInfo"/>, or null if the argument name is not present.</returns>
        public ArgumentInfo? this[string argumentName]
        {
            get
            {
                if (_innerHash.ContainsKey(argumentName))
                    return _innerHash[argumentName];

                return null;
            }
        }
    }
}
