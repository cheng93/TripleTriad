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
                ThisAssembly, // Web
                typeof(ICoinTossService).GetTypeInfo().Assembly, //Logic
            };

            builder.RegisterAssemblyTypes(assemblies).AsImplementedInterfaces();
        }
    }
}