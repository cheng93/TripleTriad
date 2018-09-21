using System.Reflection;
using Autofac;
using TripleTriad.Logic.CoinToss;

namespace TripleTriad.Web.IoC
{
    public class MainModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assemblies = new[]
            {
                ThisAssembly,
                typeof(ICoinTossService).GetTypeInfo().Assembly
            };

            builder.RegisterAssemblyTypes(assemblies).AsImplementedInterfaces();
        }
    }
}