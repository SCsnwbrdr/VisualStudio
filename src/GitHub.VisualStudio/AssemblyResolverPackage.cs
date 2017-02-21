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
        // list of assemblies that should be resolved by name only
        static readonly string[] ourAssemblies =
        {
            "GitHub.Exports",
            "GitHub.Exports.Reactive",
            "GitHub.VisualStudio.UI",
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
                        Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Not resolving '{0}' to '{1}'.", e.Name, targetName.FullName));
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
