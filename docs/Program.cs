using System;
using System.Diagnostics;
using System.IO;

var projectDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", ".."));

Process.Start(new ProcessStartInfo {
    FileName = "dotnet",
    Arguments = "docfx docfx.json --serve --open-browser",
    UseShellExecute = false,
    WorkingDirectory = projectDirectory
})!.WaitForExit();

