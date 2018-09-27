using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Optional;

namespace DNResource.AIterator
{
    public interface IAsyncIterator<T>
    {
        Task<Option<Tuple<T, IAsyncIterator<T>>>> NextAsync();
    }
}