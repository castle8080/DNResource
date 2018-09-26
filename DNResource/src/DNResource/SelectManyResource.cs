using System;
using System.Threading;
using System.Threading.Tasks;

namespace DNResource
{
    internal class SelectManyResource<T, I> : AbstractResource<T>
    {
        private IResource<I> innerResource;

        private Func<I, Task<IResource<T>>> mapper;

        public SelectManyResource(IResource<I> innerResource, Func<I, Task<IResource<T>>> mapper)
        {
            this.innerResource = innerResource;
            this.mapper = mapper;
        }

        public override Task<R> UnsafeEvaluateAsync<R>(Func<T, Task<R>> callback)
        {
            return innerResource.UnsafeEvaluateAsync<R>(async (innerValue) =>
            {
                var outerResource = await mapper(innerValue);
                return await outerResource.UnsafeEvaluateAsync<R>(outerValue => callback(outerValue));
            });
        }
    }
}
