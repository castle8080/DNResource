using System;
using System.Threading;
using System.Threading.Tasks;

namespace DNResource
{

    internal class SelectResource<T, I> : AbstractResource<T>
    {
        private IResource<I> innerResource;

        private Func<I, Task<T>> mapper;

        public SelectResource(IResource<I> innerResource, Func<I, Task<T>> mapper)
        {
            this.innerResource = innerResource;
            this.mapper = mapper;
        }

        public override Task<R> UnsafeEvaluateAsync<R>(Func<T, Task<R>> callback)
        {
            return innerResource.UnsafeEvaluateAsync<R>(async (innerValue) =>
            {
                var outerValue = await mapper(innerValue);
                return await callback(outerValue);
            });
        }
    }
}
