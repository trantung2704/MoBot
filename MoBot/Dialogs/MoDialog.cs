using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

namespace EnvisageBot.Dialogs
{
    [Serializable]
    [LuisModel("68f853c1-5b5e-4c85-a1a0-cb086a473861", "23eecd3a287b44c0b946b24f6fafd441")]
    public class MoDialog : LuisDialog<object>
    {
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Sorry I dont understand you.");
            context.Wait(MessageReceived);
        }
    }
}