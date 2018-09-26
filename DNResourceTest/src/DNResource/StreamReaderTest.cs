using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

using DNResource.IO;

namespace DNResource
{
    public class UnitTest1
    {


        private Task<List<string>> GetLines(Stream stream)
        {
            return GetLines(new StreamReader(stream));
        }

        private async Task<List<string>> GetLines(TextReader reader)
        {
            var lines = new List<string>();
            while (true)
            {
                var line = await reader.ReadLineAsync();
                if (line == null)
                {
                    return lines;
                }
                else
                {
                    lines.Add(line);
                }
            }
        }

        [Fact]
        public async Task ReadFileResource()
        {

            var linesResource = StreamResource
                .FromFile("../../../test_data/test_lines_1.txt", FileMode.Open)
                .SelectAsync(GetLines);

            var countResource = linesResource.Select(lines => lines.Count);
            var firstLineResource = linesResource.Select(lines => lines.First());

            var count = await countResource.UnsafeGetAsync();
            Assert.Equal(10, count);

            var line1 = await firstLineResource.UnsafeGetAsync();
            Assert.Equal("line1", line1);
        }
    }
}
