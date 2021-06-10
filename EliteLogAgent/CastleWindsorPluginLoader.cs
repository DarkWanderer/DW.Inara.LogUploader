namespace EliteLogAgent
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using DW.ELA.Interfaces;
    using NLog;

    internal class CastleWindsorPluginLoader : IPluginManager
    {
        private static readonly ILogger Log = LogManager.GetCurrentClassLogger();

        private readonly IWindsorContainer container;

        public CastleWindsorPluginLoader(IWindsorContainer container)
        {
            this.container = container;
        }

        public IReadOnlyCollection<string> LoadedPluginIds => Plugins.Select(p => p.PluginId).ToArray();

        public IReadOnlyCollection<IPlugin> LoadedPlugins => Plugins.ToArray();

        public IList<IPlugin> Plugins => container.ResolveAll<IPlugin>();

        public IPlugin GetPluginById(string pluginId) => Plugins.SingleOrDefault(p => p.PluginId == pluginId);

        public void LoadPlugin(string pluginAssemblyName)
        {
            try
            {
                string assemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (!pluginAssemblyName.EndsWith(".dll"))
                    pluginAssemblyName += ".dll";

                container.Register(Classes
                    .FromAssemblyNamed(Path.Combine(assemblyDirectory, pluginAssemblyName))
                    .BasedOn<IPlugin>()
                    .WithService
                    .FromInterface()
                    .LifestyleSingleton());
            }
            catch (FileNotFoundException e)
            {
                Log.Warn(e, $"Plugin assembly not found {pluginAssemblyName}");
            }
            catch (Exception e)
            {
                Log.Error(e, $"Error while loading plugin: {pluginAssemblyName}");
            }
        }

        public void LoadEmbeddedPlugins()
        {
            try
            {
                container.Register(Classes
                    .FromAssembly(Assembly.GetExecutingAssembly())
                    .BasedOn<IPlugin>()
                    .WithService
                    .FromInterface()
                    .LifestyleSingleton());
            }
            catch (Exception e)
            {
                Log.Error(e, "Error while loading embedded plugins");
            }
        }
    }
}
