using System;
using Microsoft.VisualStudio.Shell;

[assembly: ProvideBindingRedirection(
    AssemblyName = "GitHub.StartPage", PublicKeyToken = "bc1bd09f2901c82e", Culture = "neutral",
    OldVersionLowerBound = "0.0.0.0", OldVersionUpperBound = AssemblyVersionInformation.Version,
    NewVersion = AssemblyVersionInformation.Version,
    CodeBase = @"$PackageFolder$\GitHub.StartPage.dll")]

[assembly: ProvideBindingRedirection(
    AssemblyName = "GitHub.UI", PublicKeyToken = "bc1bd09f2901c82e", Culture = "neutral",
    OldVersionLowerBound = "0.0.0.0", OldVersionUpperBound = AssemblyVersionInformation.Version,
    NewVersion = AssemblyVersionInformation.Version,
    CodeBase = @"$PackageFolder$\GitHub.UI.dll")]

[assembly: ProvideBindingRedirection(
    AssemblyName = "GitHub.VisualStudio", PublicKeyToken = "bc1bd09f2901c82e", Culture = "neutral",
    OldVersionLowerBound = "0.0.0.0", OldVersionUpperBound = AssemblyVersionInformation.Version,
    NewVersion = AssemblyVersionInformation.Version,
    CodeBase = @"$PackageFolder$\GitHub.VisualStudio.dll")]

[assembly: ProvideBindingRedirection(
    AssemblyName = "GitHub.Exports", PublicKeyToken = "bc1bd09f2901c82e", Culture = "neutral",
    OldVersionLowerBound = "0.0.0.0", OldVersionUpperBound = AssemblyVersionInformation.Version,
    NewVersion = AssemblyVersionInformation.Version,
    CodeBase = @"$PackageFolder$\GitHub.Exports.dll")]