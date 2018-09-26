using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DNResource
{
    public static class Resource
    {
        public static IResource<T> FromResult<T>(T result)
        {
            return new BasicResource<T>(() => Task.FromResult(result), r => Task.FromResult(0));
        }

        public static IResource<T> CreateAsync<T>(Func<Task<T>> opener, Func<T, Task> closer)
        {
            return new BasicResource<T>(opener, closer);
        }

        public static IResource<T> Flatten<T>(this IResource<IResource<T>> nestedResource)
        {
            return nestedResource.SelectMany(ir => ir);
        }

        public static IResource<T> Zip<R1,R2,T>(this IResource<R1> resource1, IResource<R2> resource2, Func<R1, R2, T> reducer)
        {
            return resource1.SelectMany(i1 => resource2.Select(i2 => reducer(i1, i2)));
        }

        public static IResource<T> ZipAsync<R1,R2,T>(this IResource<R1> resource1, IResource<R2> resource2, Func<R1, R2, Task<T>> reducer)
        {
            return resource1.SelectMany<T>(i1 => resource2.SelectAsync<T>(i2 => reducer(i1, i2)));
        }

        public static IResource<List<T>> Join<T>(List<IResource<T>> resources)
        {
            return Aggregate<T, List<T>>(
                resources,
                new List<T>(),
                (items, item) =>
                {
                    items.Add(item);
                    return items;
                }
            );
        }

        public static IResource<R> Aggregate<T, R>(
            IEnumerable<IResource<T>> resources,
            R initialValue,
            Func<R, T, R> aggregator)
        {
            var resourceArray = resources.ToArray();
            return AggregateHelper<T, R>(
                resourceArray,
                resourceArray.Length - 1,
                initialValue,
                aggregator);
        }

        private static IResource<R> AggregateHelper<T, R>(
            IResource<T>[] resources,
            int pos,
            R initialValue,
            Func<R, T, R> aggregator)
        {
            if (pos < 0)
            {
                return Resource.FromResult(initialValue);
            }
            else
            {
                var currentResource = resources[pos];
                var combinedResource = AggregateHelper(resources, pos - 1, initialValue, aggregator);
                return combinedResource.Zip(currentResource, aggregator);
            }
        }

        public static IResource<T> Create<T>(Func<T> opener, Action<T> closer)
        {
            return CreateAsync(
                () => Task.FromResult(opener()),
                t =>
                {
                    closer(t);
                    return Task.FromResult(0);
                }
            );            
        }
    }
}