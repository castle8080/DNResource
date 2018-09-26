using System;
using System.Threading;
using System.Threading.Tasks;

namespace DNResource
{
    internal class BasicResource<T> : AbstractResource<T>
    {
        private Func<Task<T>> factory;
        private Func<T, Task> closer;

        public BasicResource(Func<Task<T>> factory, Func<T, Task> closer)
        {
            this.factory = factory;
            this.closer = closer;
        }

        public override async Task<R> UnsafeEvaluate<R>(Func<T, Task<R>> callback)
        {
            var resource = await this.factory();
            try
            {
                return await callback(resource);
            }
            finally
            {
                await this.closer(resource);
            }
        }
    }
}
