using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Optional;
using Optional.Linq;

namespace DNResource.AIterator
{
    public static class StreamAsyncIterator
    {
        
        public static IAsyncIterator<string> GetLineIteratorAsync(TextReader reader)
        {
            return AsyncIterator.Create<TextReader, string>(reader, async (innerReader) =>
            {
                var line = await innerReader.ReadLineAsync();
                return line.SomeNotNull().Select(l => Tuple.Create(l, innerReader));
            });
        }
    }
}