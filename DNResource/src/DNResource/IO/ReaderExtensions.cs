using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DNResource.IO
{

    public static class ReaderExtensions
    {
        public static IEnumerator<string> GetLines(this TextReader reader)
        {
            while (true)
            {
                var line = reader.ReadLine();
                if (line == null)
                {
                    yield break;
                }
                else {
                    yield return line;
                }
            }
        }
    }
}