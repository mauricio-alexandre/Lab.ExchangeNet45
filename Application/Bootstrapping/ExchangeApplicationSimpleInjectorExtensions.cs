using System.Linq;
using System.Reflection;
using Lab.ExchangeNet45.Application.Shared;
using MediatR;
using NHibernate;
using SimpleInjector;

namespace Lab.ExchangeNet45.Application.Bootstrapping
{
    public static class ExchangeApplicationSimpleInjectorExtensions
    {
        public static void RegisterExchangeApplication(this Container container)
        {
            // Persistence
            container.RegisterSingleton(() => new NHibernateSessionFactoryFactory().CreateSqLite());
            container.Register(() => container.GetInstance<ISessionFactory>().OpenSession(), Lifestyle.Scoped);

            Assembly applicationAssembly = typeof(NHibernateSessionFactoryFactory).Assembly;

            // Repositories
            container.RegisterRepositories(applicationAssembly);

            // Adds pipeline behaviors to MediatR
            container.Collection.Append(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));
            container.Collection.Append(typeof(IPipelineBehavior<,>), typeof(TransactionPipelineBehavior<,>));
        }

        private static void RegisterRepositories(this Container container, Assembly assembly)
        {
            var repositories =
                from type in assembly.GetTypes()
                where type.IsClass && !type.IsAbstract && !type.IsInterface
                where type.Name.EndsWith("Repository")
                from interfaceType in type.GetInterfaces().Where(i => i.Assembly.Equals(assembly))
                select new { Service = interfaceType, Implementation = type };

            foreach (var repository in repositories)
            {
                container.Register(repository.Service, repository.Implementation);
            }
        }
    }
}
