using System;
using System.Text.RegularExpressions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.FormFlow.Advanced;

namespace MoBot.Forms
{
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

    [Serializable]
    public class ClientRequest
    {
        [Prompt("Tell me more about the App you would like ")]
        public string Description { get; set; }

        [Optional]
        public string OldPhoneNumber { get; set; }
        
        [Prompt("Is {OldPhoneNumber} the best number to contact you on?")]
        public bool UseOldPhoneNumber { get; set; }

        [Prompt("What is the best phone number to contact you on?")]
        public string NewPhoneNumber { get; set; }

        [Optional]
        public string OldEmail { get; set; }
        
        [Prompt("Is {OldEmail} the best email to contact you on?")]
        public bool UseOldEmail { get; set; }

        [Prompt("What is the best email address to send you the quotes to?")]
        public string NewEmail { get; set; }

        public static IForm<ClientRequest> BuildForm()
        {
            OnCompletionAsyncDelegate<ClientRequest> processRequest = async (context, state) => { await context.PostAsync("I have sent your request. You will hear from me very soon!"); };
            return new FormBuilder<ClientRequest>()
                .Field(nameof(Description))
                .Message("Thanks! I am getting smarter by the day... but for this request I am going to get some humans to work this one out for you.")
                .Field(nameof(OldPhoneNumber))
                .Field(new FieldReflector<ClientRequest>(nameof(UseOldPhoneNumber))
                           .SetActive(state => !string.IsNullOrEmpty(state.OldPhoneNumber)))
                .Field(new FieldReflector<ClientRequest>(nameof(NewPhoneNumber))
                           .SetActive((state) => string.IsNullOrEmpty(state.OldPhoneNumber) || !state.UseOldPhoneNumber)
                           .SetValidate(async (state, value) =>
                                        {
                                            var phoneRegex = @"^\d{8,11}$";

                                            var isMatch = Regex.IsMatch(value.ToString(), phoneRegex, RegexOptions.IgnoreCase);
                                            var result = new ValidateResult
                                                         {
                                                             IsValid = isMatch,
                                                             Value = value
                                                         };

                                            if (!isMatch)
                                            {
                                                result.Feedback = "Try writing in an 8 digit format or if international then 10 digits without the +";
                                            }
                                            return result;
                                        }))
                .Field(nameof(OldEmail))
                .Field(new FieldReflector<ClientRequest>(nameof(UseOldEmail))
                           .SetActive(state => !string.IsNullOrEmpty(state.OldEmail)))
                .Field(new FieldReflector<ClientRequest>(nameof(NewEmail))
                           .SetActive((state) => string.IsNullOrEmpty(state.OldEmail) || !state.UseOldEmail)
                           .SetValidate(async (state, value) =>
                                        {
                                            var emailRegex = @"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$";

                                            var isMatch = Regex.IsMatch(value.ToString(), emailRegex, RegexOptions.IgnoreCase);
                                            var result = new ValidateResult
                                            {
                                                IsValid = isMatch,
                                                Value = value
                                            };

                                            if (!isMatch)
                                            {
                                                result.Feedback = "Email is invalid";
                                            }
                                            return result;

                                        }))
                .OnCompletion(processRequest)
                .Build();
        }
    }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
}