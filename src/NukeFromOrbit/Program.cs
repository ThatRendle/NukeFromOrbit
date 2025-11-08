using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Threading.Tasks;
using NukeFromOrbit;


var command = CreateRootCommand();
        int result = await command.Parse(args).InvokeAsync();

    static RootCommand CreateRootCommand()
    {
        var yesOption = new Option<bool>("--yes", "-y")
        {
            DefaultValueFactory = _ => false,
            Description = "Don't ask for confirmation, just nuke it."
        };
        var dryRunOption = new Option<bool>("--dry-run", "-n")
        {
            DefaultValueFactory = _ => false,
            Description = "List items that will be nuked but don't nuke them."
        };
        var workingDirectoryArgument = new Argument<string>("workingDirectory")
        {
            DefaultValueFactory = _ => Environment.CurrentDirectory,
            Description = "The working directory."
        };
            
        var command = new RootCommand
        {
            yesOption,
            dryRunOption,
            workingDirectoryArgument,
        };

        command.Description = "Dust off and nuke bin and obj directories from orbit. It's the only way to be sure.";
            
        command.SetAction(async (parseResult, token) =>
        {
            var yes = parseResult.GetRequiredValue(yesOption);
            var dryRun = parseResult.GetRequiredValue(dryRunOption);
            var workingDirectory = parseResult.GetRequiredValue(workingDirectoryArgument);
            await Nuke(yes, dryRun, workingDirectory);
        });

        return command;
    }

    static async Task Nuke(bool yes, bool dryRun, string workingDirectory)
    {
        var nuker = await Nuker.CreateAsync(workingDirectory);
        var items = nuker.GetItemsToBeNuked();
        if (items.Count == 0)
        {
            Console.WriteLine("No bin or obj directories found.");
            Console.WriteLine();
            return;
        }
        if (dryRun)
        {
            OutputDryRun(items);
        }
        else if (yes || Confirm(items))
        {
            Console.WriteLine();
            nuker.NukeItems(items);
        }
    }

    static bool Confirm(IReadOnlyCollection<DeleteItem> items)
    {
        OutputDryRun(items);
        Console.Write("Do you want to delete these items? Y/N: ");
        var yn = Console.ReadLine();
        return "Y".Equals(yn, StringComparison.CurrentCultureIgnoreCase);
    }

    static void OutputDryRun(IEnumerable<DeleteItem> items)
    {
        Console.WriteLine();
        foreach (var item in items)
        {
            Console.WriteLine(item.Path);
        }
        Console.WriteLine();
    }