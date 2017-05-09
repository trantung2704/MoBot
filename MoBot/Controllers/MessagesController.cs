using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;

namespace MoBot.Controllers
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                using (var scope = DialogModule.BeginLifetimeScope(Conversation.Container, activity))
                {
                    await Conversation.SendAsync(activity, () => scope.Resolve<IDialog<object>>());
                }
            }
            else
            {
                await HandleSystemMessage(activity);
            }
            // Return response
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private async Task<Activity> HandleSystemMessage(Activity activity)
        {
            if (activity.Type == ActivityTypes.DeleteUserData)
            {
                activity.Type = ActivityTypes.Message;
                activity.Text = "Delete User Data";
                using (var scope = DialogModule.BeginLifetimeScope(Conversation.Container, activity))
                {
                    await Conversation.SendAsync(activity, () => scope.Resolve<IDialog<object>>());
                }
            }
            else if (activity.Type == ActivityTypes.ConversationUpdate)
            {
                if (activity.MembersAdded.Any(m => m.Id == activity.Recipient.Id))
                {
                    activity.Type = ActivityTypes.Message;
                    activity.Text = "Hi";
                    using (var scope = DialogModule.BeginLifetimeScope(Conversation.Container, activity))
                    {
                        await Conversation.SendAsync(activity, () => scope.Resolve<IDialog<object>>());
                    }
                }
            }
            else if (activity.Type == ActivityTypes.ContactRelationUpdate)
            {
                if (activity.MembersAdded.Any(m => m.Id == activity.Recipient.Id))
                {
                    activity.Type = ActivityTypes.Message;
                    activity.Text = "Hi";
                    using (var scope = DialogModule.BeginLifetimeScope(Conversation.Container, activity))
                    {
                        await Conversation.SendAsync(activity, () => scope.Resolve<IDialog<object>>());
                    }
                }
            }
            else if (activity.Type == ActivityTypes.Typing)
            {
            }
            else if (activity.Type == ActivityTypes.Ping)
            {
                activity.Type = ActivityTypes.Message;
                activity.Text = "Hi";
                using (var scope = DialogModule.BeginLifetimeScope(Conversation.Container, activity))
                {
                    await Conversation.SendAsync(activity, () => scope.Resolve<IDialog<object>>());
                }
            }

            return null;
        }
    }
}