using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NukeFromOrbit.Tests
{
    public class NukerTests
    {
        private static readonly string[] Files =
        {
            @"/Users/rendle/Fake/src/FakeFake.csproj",
            @"/Users/rendle/Fake/src/FakeFake.cs",
            @"/Users/rendle/Fake/src/Fake/bin/Debug/Fake.dll",
            @"/Users/rendle/Fake/src/Fake/obj/project.assets.json",
            @"/Users/rendle/Fake/src/Fake/node_modules/foo/bin/foo.js",
        };
        
        [Fact]
        public async Task GetsItemsToBeNuked()
        {
            var fakeFileSystem = FakeFileSystem.Fake(Files);
            var fakeGitFileList = new FakeGitFileList();
            var fakeConsole = new FakeConsole();
            
            var nuker = await Nuker.CreateAsync(@"/Users/rendle/Fake", fakeFileSystem, fakeGitFileList, fakeConsole);

            var actual = nuker.GetItemsToBeNuked();
            Assert.Contains(actual, i => i is { Path: @"/Users/rendle/Fake/src/Fake/bin", Type: ItemType.Directory });
            Assert.Contains(actual, i => i is { Path: @"/Users/rendle/Fake/src/Fake/obj", Type: ItemType.Directory });
            Assert.DoesNotContain(actual, i => i.Path == @"/Users/rendle/Fake/src/Fake/node_modules/foo/bin");
        }
        
        [Fact]
        public async Task NukesItems()
        {
            var fakeFileSystem = FakeFileSystem.Fake(Files);
            var fakeGitFileList = new FakeGitFileList();
            var fakeConsole = new FakeConsole();
            
            var nuker = await Nuker.CreateAsync(@"/Users/rendle/Fake", fakeFileSystem, fakeGitFileList, fakeConsole);

            var actual = nuker.GetItemsToBeNuked();
            nuker.NukeItems(actual);
            
            var directories = fakeFileSystem.Directory.GetDirectories("/Users/rendle/Fake/src/Fake").Select(Path.GetFileName).ToArray();
            Assert.DoesNotContain("bin", directories);
            Assert.DoesNotContain("obj", directories);

            directories = fakeFileSystem.Directory.GetDirectories("/Users/rendle/Fake/src/Fake/node_modules/foo").Select(Path.GetFileName).ToArray();
            Assert.Contains("bin", directories);
        }
    }
}