using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NukeFromOrbit.Tests
{
    public class NukerGitTests
    {
        private static readonly string[] Files =
        {
            @"/Users/rendle/Fake/src/Fake/Fake.csproj",
            @"/Users/rendle/Fake/src/Fake/Fake.cs",
            @"/Users/rendle/Fake/src/Fake/bin/Debug/Fake.dll",
            @"/Users/rendle/Fake/src/Fake/bin/IAmVersionControlled.txt",
            @"/Users/rendle/Fake/src/Fake/obj/project.assets.json",
        };
        
        [Fact]
        public async Task GetsItemsToBeNuked()
        {
            var fakeFileSystem = FakeFileSystem.Fake(Files);
            
            var gitFileList = FakeFileList();

            var fakeConsole = new FakeConsole();

            var nuker = await Nuker.CreateAsync(@"/Users/rendle/Fake", fakeFileSystem, gitFileList, fakeConsole);

            var actual = nuker.GetItemsToBeNuked();
            Assert.Contains(actual, i => i.Path == @"/Users/rendle/Fake/src/Fake/bin/Debug/Fake.dll" && i.Type == ItemType.File);
            Assert.Contains(actual, i => i.Path == @"/Users/rendle/Fake/src/Fake/obj" && i.Type == ItemType.Directory);
            Assert.DoesNotContain(actual, i => i.Path == @"/Users/rendle/Fake/src/Fake/bin/IAmVersionControlled.txt");
        }

        private static IGitFileList FakeFileList()
        {
            var gitFiles = new HashSet<string>(Files, StringComparer.OrdinalIgnoreCase);
            gitFiles.Remove(@"/Users/rendle/Fake/src/Fake/bin/Debug/Fake.dll");
            gitFiles.Remove(@"/Users/rendle/Fake/src/Fake/obj/project.assets.json");

            var gitFileList = new FakeGitFileList(gitFiles);
            return gitFileList;
        }

        [Fact]
        public async Task NukesItems()
        {
            var fakeFileSystem = FakeFileSystem.Fake(Files);
            var gitFileList = FakeFileList();
            var fakeConsole = new FakeConsole();
            
            var nuker = await Nuker.CreateAsync(@"/Users/rendle/Fake", fakeFileSystem, gitFileList, fakeConsole);

            var actual = nuker.GetItemsToBeNuked();
            nuker.NukeItems(actual);

            var entries = fakeFileSystem.Directory.GetDirectories("/Users/rendle/Fake/src/Fake").Select(Path.GetFileName).ToArray();
            Assert.Contains("bin", entries);
            Assert.DoesNotContain("obj", entries);
            
            entries = fakeFileSystem.Directory.GetFiles("/Users/rendle/Fake/src/Fake/bin").Select(Path.GetFileName).ToArray();
            Assert.Contains("IAmVersionControlled.txt", entries);
            entries = fakeFileSystem.Directory.GetFiles("/Users/rendle/Fake/src/Fake/bin/Debug").Select(Path.GetFileName).ToArray();
            Assert.DoesNotContain("Fake.dll", entries);

            // fakeFileSystem.Directory.DidNotReceive().Delete("/Users/rendle/Fake/src/Fake/bin", true);
            // fakeFileSystem.File.Received().Delete(@"/Users/rendle/Fake/src/Fake/bin/Debug/Fake.dll");
            // fakeFileSystem.Directory.Received().Delete(@"/Users/rendle/Fake/src/Fake/obj", true);
        }
    }
}