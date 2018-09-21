using System.Reflection;
using Autofac;
using TripleTriad.Logic.ToinCoss;

namespace TripleTriad.Web.IoC
{
    public class MainModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assemblies = new[]
            {
                ThisAssembly,
                typeof(IToinCossService).GetTypeInfo().Assembly
            };

            builder.RegisterAssemblyTypes(assemblies).AsImplementedInterfaces();
        }
    }
}