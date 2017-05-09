using System.Web.Http;
using Autofac;
using EnvisageBot.Dialogs;
using Microsoft.Bot.Builder.Dialogs;

namespace MoBot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MoDialog>()
                   .As<IDialog<object>>()
                   .InstancePerDependency();

            builder.Update(Conversation.Container);

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
