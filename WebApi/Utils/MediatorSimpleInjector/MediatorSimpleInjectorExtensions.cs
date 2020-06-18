using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MediatR;
using MediatR.Pipeline;
using SimpleInjector;
// github.com/jbogard/MediatR/blob/master/samples/MediatR.Examples.SimpleInjector/Program.cs

namespace Lab.ExchangeNet45.WebApi.Utils.MediatorSimpleInjector
{
    public static class MediatorSimpleInjectorExtensions
    {
        public static void RegisterMediatR(this Container container, Assembly assembly, params Assembly[] assemblies)
        {
            Assembly[] allAssemblies = assemblies != null
                ? new[] { assembly }.Concat(assemblies).ToArray()
                : new[] { assembly };

            RegisterMediatR(container, allAssemblies);
        }

        public static void RegisterMediatR(this Container container, IEnumerable<Assembly> assemblies)
        {
            Assembly mediatorAssembly = typeof(IMediator).GetTypeInfo().Assembly;

            Assembly[] allAssemblies = assemblies != null
                ? new[] { mediatorAssembly }.Concat(assemblies).ToArray()
                : new[] { mediatorAssembly };

            container.RegisterSingleton<IMediator, Mediator>();
            container.Register(typeof(IRequestHandler<,>), allAssemblies);
            container.RegisterHandlers(typeof(INotificationHandler<>), allAssemblies);
            container.RegisterPipelineBehavior(allAssemblies);
            container.RegisterSingleton(() => new ServiceFactory(container.GetInstance));
        }

        private static void RegisterHandlers(this Container container, Type collectionType, Assembly[] assemblies)
        {
            // we have to do this because by default, generic type definitions (such as the Constrained Notification Handler) won't be registered
            IEnumerable<Type> handlerTypes = container.GetTypesToRegister(collectionType, assemblies, new TypesToRegisterOptions
            {
                IncludeGenericTypeDefinitions = true,
                IncludeComposites = false
            });

            container.Collection.Register(collectionType, handlerTypes);
        }

        private static void RegisterPipelineBehavior(this Container container, Assembly[] assemblies)
        {
            Type[] defaultPipelineTypes =
            {
                typeof(RequestPreProcessorBehavior<,>),
                typeof(RequestPostProcessorBehavior<,>),
                typeof(GenericPipelineBehavior<,>)
            };

            //IEnumerable<Type> allOtherPipelineTypes = assemblies
            //    .Select(assembly => assembly.GetTypes())
            //    .SelectMany(types => types)
            //    .Where(type => type.IsClass && type.IsGenericTypeDefinition && !type.IsAbstract && !type.IsInterface)
            //    .Where(type => !defaultPipelineTypes.Contains(type.GetGenericTypeDefinition()))
            //    .Where
            //    (
            //        type => type
            //            .GetInterfaces()
            //            .Where(interfaceType => interfaceType.IsGenericType)
            //            .Any(interfaceType => interfaceType.GetGenericTypeDefinition() == typeof(IPipelineBehavior<,>))
            //    );

            container.Collection.Register(typeof(IPipelineBehavior<,>), defaultPipelineTypes/*.Concat(allOtherPipelineTypes)*/);
            container.Collection.Register(typeof(IRequestPreProcessor<>), new[] { typeof(GenericRequestPreProcessor<>) });
            container.Collection.Register(typeof(IRequestPostProcessor<,>), new[] { typeof(GenericRequestPostProcessor<,>) });
        }
    }
}