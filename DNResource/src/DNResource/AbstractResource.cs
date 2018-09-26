using System;
using System.Threading;
using System.Threading.Tasks;

namespace DNResource
{

    public abstract class AbstractResource<T> : IResource<T>
    {
        public IResource<O> Map<O>(Func<T, O> f)
        {
            return new MapResource<O, T>(this, i => Task.FromResult(f(i)));
        }

        public IResource<O> MapAsync<O>(Func<T, Task<O>> f)
        {
            return new MapResource<O, T>(this, f);
        }

        public IResource<O> FlatMap<O>(Func<T, IResource<O>> f)
        {
            return new FlatMapResource<O, T>(this, i => Task.FromResult(f(i)));
        }

        public IResource<O> FlatMapAsync<O>(Func<T, Task<IResource<O>>> f)
        {
            return new FlatMapResource<O, T>(this, f);
        }

        public Task<T> UnsafeGet()
        {
            return UnsafeEvaluate(resource => Task.FromResult(resource));
        }

        public abstract Task<R> UnsafeEvaluate<R>(Func<T, Task<R>> callback);

    }
}
