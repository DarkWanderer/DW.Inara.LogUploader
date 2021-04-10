namespace EliteLogAgent
{
    using Castle.Facilities.Logging;
    using Castle.MicroKernel.Registration;
    using Castle.Services.Logging.NLogIntegration;
    using Castle.Windsor;
    using DW.ELA.Controller;
    using DW.ELA.Interfaces;
    using DW.ELA.Utility;
    using EliteLogAgent.Autorun;
    using EliteLogAgent.Deployment;

    internal static class ContainerBootstrapper
    {
        internal static void Initalize(IWindsorContainer container)
        {
            // Initalize infrastructure classes - NLog, Windsor
            container.AddFacility<LoggingFacility>(f => f.LogUsing<NLogFactory>().ConfiguredExternally());
            container.Register(
                Component.For<IPathManager>().ImplementedBy<DataPathManager>().LifestyleSingleton(),
                Component.For<ISettingsProvider>().ImplementedBy<FileSettingsStorage>().LifestyleSingleton(),
                Component.For<ILogSettingsBootstrapper>().ImplementedBy<NLogSettingsManager>().LifestyleTransient(),
                Component.For<IPluginManager>().ImplementedBy<CastleWindsorPluginLoader>().LifestyleSingleton(),
                Component.For<IWindsorContainer>().Instance(container),
                Component.For<IRestClientFactory>().ImplementedBy<ThrottlingRestClient.Factory>());

            // Register core classes
            container.Register(
                Component.For<ILogDirectoryNameProvider>().ImplementedBy<SavedGamesDirectoryHelper>().LifestyleSingleton(),
                Component.For<ILogRealTimeDataSource>().ImplementedBy<JournalMonitor>().LifestyleSingleton(),
                Component.For<IPlayerStateHistoryRecorder>().ImplementedBy<PlayerStateRecorder>().LifestyleSingleton());

            // Register UI classes. Need to initalize before log to enable tray icon
            container.Register(Component.For<IUserNotificationInterface>().ImplementedBy<TrayIconController>().LifestyleSingleton());

            // Different components will be used based on whether apps are portable
            if (AppInfo.IsNetworkDeployed)
                container.Register(Component.For<IAutorunManager>().ImplementedBy<ClickOnceAutorunManager>().LifestyleTransient());
            else
                container.Register(Component.For<IAutorunManager>().ImplementedBy<PortableAutorunManager>().LifestyleTransient());
        }
    }
}
