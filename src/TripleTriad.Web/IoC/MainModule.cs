using System.Reflection;
using Autofac;
using TripleTriad.Logic.CoinToss;
using TripleTriad.Requests.Messages;
using TripleTriad.SignalR;

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
                typeof(PlayerIdUserProvider).GetTypeInfo().Assembly, //SignalR
            };

            builder.RegisterAssemblyTypes(assemblies).AsImplementedInterfaces();
        }
    }
}