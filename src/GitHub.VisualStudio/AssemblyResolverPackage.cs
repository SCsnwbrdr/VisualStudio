using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Globalization;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using System.Linq;

namespace GitHub.VisualStudio
{
    [ProvideAutoLoad(VSConstants.UICONTEXT.ShellInitialized_string)]
    public class AssemblyResolverPackage : ExtensionPointPackage
    {
        // list of assembly names that should always be loaded from extension dir
        static readonly string[] ourAssemblies =
        {
            "GitHub.Api",
            "GitHub.App",
            "GitHub.CredentialManagement",
            "GitHub.Exports",
            "GitHub.Exports.Reactive",
            "GitHub.Extensions",
            "GitHub.Extensions.Reactive",
            "GitHub.UI",
            "GitHub.UI.Reactive",
            "GitHub.VisualStudio",
            "GitHub.TeamFoundation",
            "GitHub.TeamFoundation.14",
            "GitHub.TeamFoundation.15",
            "GitHub.VisualStudio.UI",
            "System.Windows.Interactivity"
        };

        readonly string extensionDir;

        public AssemblyResolverPackage()
        {
            extensionDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        protected override void Initialize()
        {
            AppDomain.CurrentDomain.AssemblyResolve += LoadAssemblyFromExtensionDir;
            base.Initialize();
        }

        protected override void Dispose(bool disposing)
        {
            AppDomain.CurrentDomain.AssemblyResolve -= LoadAssemblyFromExtensionDir;
            base.Dispose(disposing);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods")]
        Assembly LoadAssemblyFromExtensionDir(object sender, ResolveEventArgs e)
        {
            try
            {
                var name = new AssemblyName(e.Name).Name;
                var filename = Path.Combine(extensionDir, name + ".dll");
                if (!File.Exists(filename))
                {
                    return null;
                }

                var targetName = AssemblyName.GetAssemblyName(filename);

                // Resolve any exact `FullName` matches.
                if (e.Name != targetName.FullName)
                {
                    // Resolve any version of our assemblies.
                    if (!ourAssemblies.Contains(name, StringComparer.OrdinalIgnoreCase))
                    {
                        return null;
                    }

                    Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Resolving '{0}' to '{1}'.", e.Name, targetName.FullName));
                }

                Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Loading '{0}' from '{1}'.", targetName.FullName, filename));
                return Assembly.LoadFrom(filename);
            }
            catch (Exception ex)
            {
                var log = string.Format(CultureInfo.CurrentCulture,
                    "Error occurred loading {0} from {1}.{2}{3}{4}",
                    e.Name,
                    Assembly.GetExecutingAssembly().Location,
                    Environment.NewLine,
                    ex,
                    Environment.NewLine);
                Trace.WriteLine(log);
                VsOutputLogger.Write(log);
            }

            return null;
        }
    }
}
