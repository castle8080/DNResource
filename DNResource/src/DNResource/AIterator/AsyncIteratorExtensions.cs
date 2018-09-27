using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Optional;
using Optional.Linq;
using Optional.Unsafe;

namespace DNResource.AIterator
{
    public static class AsyncIteratorExtensions
    {
        public static IAsyncIterator<S> Select<T,S>(this IAsyncIterator<T> iterator, Func<T,S> mapper)
        {
            return iterator.SelectAsync(item => Task.FromResult(mapper(item)));
        }

        public static IAsyncIterator<S> SelectAsync<T,S>(this IAsyncIterator<T> iterator, Func<T,Task<S>> mapper)
        {
            return AsyncIterator.Create<IAsyncIterator<T>, S>(
                iterator,
                async (currentIterator) =>
                {
                    var result = await currentIterator.NextAsync();
                    if (result.HasValue)
                    {
                        var pair = result.ValueOrFailure();
                        var item = pair.Item1;
                        var nextIterator = pair.Item2;
                        return Tuple.Create(await mapper(item), nextIterator).Some();
                    }
                    else
                    {
                        return Option.None<Tuple<S, IAsyncIterator<T>>>();
                    }
                }
            );
        }
        
        public static IAsyncIterator<T> Where<T>(this IAsyncIterator<T> iterator, Func<T, bool> predicate)
        {
            return WhereAsync<T>(iterator, item => Task.FromResult(predicate(item)));
        }

        public static IAsyncIterator<T> WhereAsync<T>(this IAsyncIterator<T> iterator, Func<T, Task<bool>> predicate)
        {
            return AsyncIterator.Create<IAsyncIterator<T>, T>(
                iterator,
                async (currentIterator) =>
                {
                    while (true)
                    {
                        var result = await currentIterator.NextAsync();
                        if (result.HasValue)
                        {
                            var pair = result.ValueOrFailure();
                            var item = pair.Item1;
                            var nextIterator = pair.Item2;
                            if (await predicate(item))
                            {
                                return Tuple.Create(item, nextIterator).Some();
                            }
                        }
                        else
                        {
                            return Option.None<Tuple<T, IAsyncIterator<T>>>();
                        }
                    }
                }
            );  
        }
    }
}