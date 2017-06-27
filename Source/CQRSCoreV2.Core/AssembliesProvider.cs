using System.Diagnostics;

namespace CQRSCoreV2.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Web.Compilation;
    using System.Web.Hosting;

    public interface IAssembliesProvider
    {
        IEnumerable<Assembly> Assemblies { get; }
    }

    public class AssembliesProvider : IAssembliesProvider
    {
        private static readonly IAssembliesProvider Singleton = new AssembliesProvider();

        private const string AssemlyNameStartWith = "CQRS";

        private static readonly object SyncLock = new object();
        private static IEnumerable<Assembly> assemblies;

        public static IAssembliesProvider Instance
        {
            get { return Singleton; }
        }

        public IEnumerable<Assembly> Assemblies
        {
            get
            {
                if (assemblies != null)
                {
                    return assemblies;
                }

                var available = AvailableAssemblies();

                lock (SyncLock)
                {
                    if (assemblies == null)
                    {
                        assemblies = available;
                    }
                }

                return assemblies;
            }
        }

        private static IEnumerable<Assembly> AvailableAssemblies()
        {
            return HostingEnvironment.IsHosted ?
                WebAssemblies() :
                ConsoleAssemblies();
        }

        private static IEnumerable<Assembly> ConsoleAssemblies()
        {
            var path = Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().Location);

            if (path == null)
            {
                throw new InvalidOperationException("Unable to get directory location.");
            }

            var filePattern = $"{AssemlyNameStartWith}*.dll";

            var files = Directory.GetFiles(path, filePattern);

            var allAssemblies = files.Select(LoadAssemblyFile).ToList();

            allAssemblies.Add(Assembly.GetEntryAssembly());

            var result = allAssemblies.Where(a => a != null).Distinct().OrderBy(a => a.FullName).ToList();
            return result;
        }
        private static Assembly LoadAssemblyFile(string path)
        {
            try
            {
                return Assembly.LoadFile(path);
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error while loading asembly file: {0}. Exception: '{1}'. {2}", path, ex.GetType().FullName, ex.Message);
                throw;
            }
        }

        private static IEnumerable<Assembly> WebAssemblies()
        {
            var list = BuildManager.GetReferencedAssemblies()
                    .Cast<Assembly>()
                    .Where(a => !a.GlobalAssemblyCache)
                    .Where(a => !a.IsDynamic)
                    .Where(a => !a.ReflectionOnly)
                    .Where(a => a.FullName.StartsWith(AssemlyNameStartWith, StringComparison.Ordinal))
                    .ToList();

            return list;
        }
    }
}