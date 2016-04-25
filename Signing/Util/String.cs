using System;
using System.Collections.Generic;

namespace Signing.Util
{
    public class String
    {
        /// <summary>
        /// Split the given string into equals chunks
        /// </summary>
        /// <param name="str"></param>
        /// <param name="chunkSize"></param>
        /// <returns></returns>
        public IEnumerable<string> Chunk(string str, int chunkSize)
        {
            for (int i = 0; i < str.Length; i += chunkSize)
                yield return str.Substring(i, Math.Min(chunkSize, str.Length - i));
        }
    }
}
