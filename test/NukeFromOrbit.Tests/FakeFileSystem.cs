using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;

namespace NukeFromOrbit.Tests
{
    internal static class FakeFileSystem
    {
        public static IFileSystem Fake(IEnumerable<string> fileList)
        {
            var files = fileList.ToDictionary(k => k, k => new MockFileData(k));
            
            return new MockFileSystem(files);
            // MockDirectoryInfoFactory
            // mock.AddDirectory(new MockDirectoryInfo());
            // var files = fileList.ToArray();
            // var directories = AllDirectories(files).Distinct().ToArray();
            //
            // var fileSystem = Substitute.For<IFileSystem>();
            // var path = new PathWrapper(fileSystem);
            // var file = Substitute.For<IFile>();
            //
            // var directory = FakeDirectory(directories, path, files);
            //
            // fileSystem.Path.Returns(path);
            // fileSystem.Directory.Returns(directory);
            // fileSystem.File.Returns(file);
            // return fileSystem;
        }
    }
}