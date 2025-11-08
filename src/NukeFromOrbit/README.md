# nuke-from-orbit

**Description:**

Dust off and nuke bin and obj directories from orbit. It's the only way to be sure.

Does *not* delete any files that are included in Git even if they are in a `bin` or `obj` directory.

Also does not delete anything inside a `node_modules` folder, should you have such an abomination lurking in your solution, you poor sod.

**Usage:**

`nuke-from-orbit [<workingDirectory>] [options]`

**Arguments:**

`<workingDirectory>`  The working directory. Defaults to current directory.

**Options:**

```
-y, --yes       Don't ask for confirmation, just nuke it.
-n, --dry-run   List items that will be nuked but don't nuke them.
-?, -h, --help  Show help and usage information
--version       Show version information
```

You're welcome.
