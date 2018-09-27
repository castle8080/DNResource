using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Optional;

namespace DNResource.AIterator
{
    public static class AsyncIterator
    {
        public static IAsyncIterator<T> Create<S, T>(S state, Func<S, Task<Option<Tuple<T, S>>>> extractor)
        {
            return new UnfoldAsyncIterator<S, T>(state, extractor);
        }
    }
}