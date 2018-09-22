using Autofac;
using TripleTriad.BackgroundTasks.Queue;

namespace TripleTriad.Web.IoC
{
    public class BackgroundTasksModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BackgroundTaskQueue>().As<IBackgroundTaskQueue>().SingleInstance();
        }
    }
}