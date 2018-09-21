using Autofac;
using TripleTriad.Logic.CoinToss;

namespace TripleTriad.Web.IoC
{
    public class LogicModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RandomWrapper>().As<IRandomWrapper>().SingleInstance();
        }
    }
}