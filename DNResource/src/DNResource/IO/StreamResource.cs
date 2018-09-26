using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DNResource.IO
{

    public static class StreamResource
    {
        public static IResource<Stream> FromFile(string filePath, FileMode mode)
        {
            return Create(() => new FileStream(filePath, mode));
        }

        public static IResource<Stream> Create(Func<Stream> opener)
        {
            return Resource.Create(opener, s => { s.Close(); Console.WriteLine("closed stream."); });
        }
    }
}