using System.Configuration;

using Autofac;

using LPush.Data;
using LPush.Core.Data;
using LPush.Core.Caching;
using LPush.Service.Sample;
using LPush.Core.Configuration;

using LPush.Service.Basic;


namespace LPush.Web.Framework
{
    public class DependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder)
        {
            // Here we register our user mapper as a per-request singleton.
            // As this is now per-request we could inject a request scoped
            // database "context" or other request scoped services

            //data layer
            //DataSettingsManager dataSettingsManager = new DataSettingsManager();
            //DataSettings dataSettings = dataSettingsManager.LoadSettings();

            var config = ConfigurationManager.GetSection("LPushConfig") as LPushConfig;

            var dataProviderManager = new EfDataProviderManager(config.DataSettings);
            var dataProvider = dataProviderManager.LoadDataProvider();
            dataProvider.InitConnectionFactory();

            builder.Register<IReadDbContext>(c => new EfReadObjectContext(config.DataSettings.DataConnectionString[0])).
                InstancePerLifetimeScope();
            builder.Register<IWriteDbContext>(c => new EfWriteObjectContext(config.DataSettings.DataConnectionString[1])).
                InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

            //Cache
            builder.RegisterType<LPushNullCache>().As<ICacheManager>().InstancePerLifetimeScope();
            //Service
            builder.RegisterType<ExampleService>().As<IExampleService>().InstancePerLifetimeScope();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
        }
    }
}
