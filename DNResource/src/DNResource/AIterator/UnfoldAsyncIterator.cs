using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Optional;
using Optional.Linq;

namespace DNResource.Stream
{
    internal class UnfoldAsyncIterator<S, T> : IAsyncIterator<T>
    {
        private S state;
        private Func<S, Task<Option<Tuple<T, S>>>> extractor;

        public UnfoldAsyncIterator(S state, Func<S, Task<Option<Tuple<T, S>>>> extractor)
        {
            this.state = state;
            this.extractor = extractor;
        }

        public async Task<Option<Tuple<T, IAsyncIterator<T>>>> NextAsync()
        {
            var result = await extractor(state);
            return result.Select(pair => {
                var item = pair.Item1;
                var newState = pair.Item2;
                return Tuple.Create(item, (IAsyncIterator<T>) new StateExtractingAsyncIterator<S, T>(newState, extractor));
            });
        }
    }
}