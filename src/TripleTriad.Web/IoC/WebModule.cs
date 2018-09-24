using Autofac;
using TripleTriad.Web.Filters;

namespace TripleTriad.Web.IoC
{
    public class WebModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EnsurePlayerIdExistsActionFilter>();
        }
    }
}