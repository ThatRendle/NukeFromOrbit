using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NukeFromOrbit.Tests
{
    internal class FakeGitFileList : IGitFileList
    {
        private readonly HashSet<string> _list;

        public FakeGitFileList(params string[] list) : this(list.AsEnumerable())
        {
        }

        public FakeGitFileList(IEnumerable<string> list)
        {
            _list = new HashSet<string>(list.Concat(list.Select(Path.GetDirectoryName)));
        }

        public Task<HashSet<string>> GetAsync() => Task.FromResult(_list);
    }
}