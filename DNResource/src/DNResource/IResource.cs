using System;
using System.Threading;
using System.Threading.Tasks;

namespace DNResource
{
    public interface IResource<T>
    {
        IResource<O> Select<O>(Func<T, O> f);

        IResource<O> SelectAsync<O>(Func<T, Task<O>> f);

        IResource<O> SelectMany<O>(Func<T, IResource<O>> f);

        IResource<O> SelectManyAsync<O>(Func<T, Task<IResource<O>>> f);

        Task<T> UnsafeGetAsync();

        Task<R> UnsafeEvaluateAsync<R>(Func<T, Task<R>> callback);
    }
}
