using System;
using System.Threading;
using System.Threading.Tasks;

namespace DNResource
{

    public abstract class AbstractResource<T> : IResource<T>
    {
        public IResource<O> Select<O>(Func<T, O> f)
        {
            return new SelectResource<O, T>(this, i => Task.FromResult(f(i)));
        }

        public IResource<O> SelectAsync<O>(Func<T, Task<O>> f)
        {
            return new SelectResource<O, T>(this, f);
        }

        public IResource<O> SelectMany<O>(Func<T, IResource<O>> f)
        {
            return new SelectManyResource<O, T>(this, i => Task.FromResult(f(i)));
        }

        public IResource<O> SelectManyAsync<O>(Func<T, Task<IResource<O>>> f)
        {
            return new SelectManyResource<O, T>(this, f);
        }

        public Task<T> UnsafeGetAsync()
        {
            return UnsafeEvaluateAsync(resource => Task.FromResult(resource));
        }

        public abstract Task<R> UnsafeEvaluateAsync<R>(Func<T, Task<R>> callback);
    }
}
