using System;
using System.Linq;
using System.Reflection;
using Autofac;
using MediatR;
using MediatR.Pipeline;
using TripleTriad.Requests.Messages;

namespace TripleTriad.Web.IoC
{
    public class RequestsModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var mediatrTypes = new[]
            {
                typeof(IRequestHandler<,>),
                typeof(IRequestHandler<>),
                typeof(INotificationHandler<>),
                typeof(IRequestPreProcessor<>),
                typeof(IRequestPostProcessor<,>),
                typeof(IPipelineBehavior<,>)
            };

            var assemblies = new[]
            {
                typeof(IMessageFactory<>).GetTypeInfo().Assembly
            };

            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => !mediatrTypes.Any(p => IsAssignableToGenericType(t, p)))
                .AsImplementedInterfaces();
        }

        private static bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                {
                    return true;
                }
            }

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }

            Type baseType = givenType.BaseType;
            if (baseType == null)
            {
                return false;
            }

            return IsAssignableToGenericType(baseType, genericType);
        }
    }
}