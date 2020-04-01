using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Quickstart.AspNetCore.Handlers
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
                        replyMarkup: new ReplyKeyboardRemove()
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
    }
}