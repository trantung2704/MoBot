using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using MoBot;
using MoBot.Forms;

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

        [LuisIntent("Greeting")]
        public async Task Greeting(IDialogContext context, LuisResult result)
        {
            bool isFirstTime;
            if (!context.UserData.TryGetValue(DataKeyManager.IsFirstTime, out isFirstTime)
                || !isFirstTime)
            {
                await context.PostAsync("Hi Toby, I am Mo' your quote bot");
                context.UserData.SetValue(DataKeyManager.IsFirstTime, true);
            }

            await context.PostAsync("I know all the top App developement providers in brisbane By answering a few quick questions I can find you who is best suited to your job and get a price - sometimes even instantly!");

            context.Wait(MessageReceived);
        }

        [LuisIntent("NewRequest")]
        public async Task NewRequest(IDialogContext context, LuisResult result)
        {
            string oldPhone;
            context.UserData.TryGetValue(DataKeyManager.OldPhone, out oldPhone);

            string oldEmail;
            context.UserData.TryGetValue(DataKeyManager.OldEmail, out oldEmail);


            var request = new ClientRequest();
            var requestForm = new FormDialog<ClientRequest>(request,
                                                            ClientRequest.BuildForm,
                                                            FormOptions.PromptInStart,
                                                            new List<EntityRecommendation>
                                                            {
                                                               new EntityRecommendation(nameof(request.OldPhoneNumber), entity: oldPhone),
                                                               new EntityRecommendation(nameof(request.OldEmail), entity: oldEmail)
                                                            });

            context.Call(requestForm, AfterNewRequestForm);
        }

        [LuisIntent("DeleteUserData")]
        public async Task DeleteUserData(IDialogContext context, LuisResult result)
        {
            context.UserData.Clear();
            await context.PostAsync("User data has been deleted");
            context.Wait(MessageReceived);
        }

        private async Task AfterNewRequestForm(IDialogContext context, IAwaitable<ClientRequest> result)
        {
            var clientRequest = await result;

            if (!string.IsNullOrEmpty(clientRequest.NewPhoneNumber))
            {
                context.UserData.SetValue(DataKeyManager.OldPhone, clientRequest.NewPhoneNumber);
            }

            if (!string.IsNullOrEmpty(clientRequest.NewEmail))
            {
                context.UserData.SetValue(DataKeyManager.OldEmail, clientRequest.NewEmail);
            }

            var phone = string.IsNullOrEmpty(clientRequest.NewPhoneNumber) ? clientRequest.OldEmail : clientRequest.NewPhoneNumber; 
            var email = string.IsNullOrEmpty(clientRequest.NewEmail) ? clientRequest.OldEmail : clientRequest.NewEmail;

            await new SendEmail().SendRequestMail(clientRequest.Description, phone, email);
            context.Wait(MessageReceived);            
        }
    }
}