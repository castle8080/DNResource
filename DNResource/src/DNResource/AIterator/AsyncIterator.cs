using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Optional;

namespace DNResource.Stream
{
    public static class AsyncIterator
    {
        public static IAsyncIterator<T> Create<S, T>(S state, Func<S, Task<Option<Tuple<T, S>>>> extractor)
        {
            return new UnfoldAsyncIterator(state, extractor);
        }
    }
}