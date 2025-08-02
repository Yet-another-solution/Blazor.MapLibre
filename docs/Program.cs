using System;
using System.Diagnostics;

Process.Start(new ProcessStartInfo {
    FileName = "dotnet",
    Arguments = "docfx serve dist --open-browser",
    UseShellExecute = false,
    WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory
})!.WaitForExit();