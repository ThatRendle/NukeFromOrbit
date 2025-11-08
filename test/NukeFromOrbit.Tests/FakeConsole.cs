using System.Collections.Generic;

namespace NukeFromOrbit.Tests;

public class FakeConsole : IConsole
{
    public List<string> WrittenLines { get; } = new();
    public void WriteLine(string line) => WrittenLines.Add(line);
}