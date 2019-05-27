using System.Threading.Tasks;
using Autofac;
using TripleTriad.SignalR;

namespace TripleTriad.Web.IoC
{
    public class SignalRModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConnectionIdStore>().As<IConnectionIdStore>().SingleInstance();
        }
    }
}