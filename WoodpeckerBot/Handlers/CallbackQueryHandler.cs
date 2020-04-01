using System.Threading.Tasks;
using System.Collections.Generic;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace WoodpeckerBot.Handlers
{
    public class CallbackQueryHandler : IUpdateHandler
    {
        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next)
        {
            CallbackQuery cq = context.Update.CallbackQuery;

            if (cq.Data.StartsWith("start/TREE"))
            {

                await context.Bot.Client.SendTextMessageAsync(
                        context.Update.CallbackQuery.Message.Chat,
                        "Send your location, please",
                        ParseMode.Markdown,
                        replyMarkup: CreateTreeMarkup()
                    );
            }
            else
            {
                await context.Bot.Client.SendTextMessageAsync(
                        context.Update.CallbackQuery.Message.Chat,
                        "Try again",
                        ParseMode.Markdown,
                        replyMarkup: new ReplyKeyboardRemove()
                    );
            }
            await next(context);
        }

        private ReplyKeyboardMarkup CreateTreeMarkup() =>
                new ReplyKeyboardMarkup(new List<KeyboardButton>()
                    {
                        new KeyboardButton("My location"){ RequestLocation = true },
                        new KeyboardButton("Back")
                    }
                )
                { ResizeKeyboard = true}
            ;
        
    }
}