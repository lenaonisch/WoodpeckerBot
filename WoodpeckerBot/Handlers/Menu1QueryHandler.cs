using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using IBWT.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;

namespace WoodpeckerBot.Handlers
{
    public class Menu1QueryHandler : IUpdateHandler
    {
        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            CallbackQuery cq = context.Update.CallbackQuery;

            /*   await context.Bot.Client.SendTextMessageAsync(
                   cq.Message.Chat,
                   context.Items["History"].ToString() + " and last item = " +  context.Items["State"].ToString(),
                   replyMarkup: new InlineKeyboardMarkup(
                       new InlineKeyboardButton[]
                       {
                           InlineKeyboardButton.WithCallbackData("menu2", "menu2::"),
                           InlineKeyboardButton.WithCallbackData("back", "back::")
                       }

                   ),
                   cancellationToken: cancellationToken
               );
               */

            switch (cq.Data)
            {
                case "menu1::TREE":
                    await context.Bot.Client.SendTextMessageAsync(
                        context.Update.CallbackQuery.Message.Chat,
                        "Send your location, please",
                        ParseMode.Markdown,
                        replyMarkup: CreateTreeMarkup()
                    );
                    break;
                case "menu1::HOLE":
                    await context.Bot.Client.SendTextMessageAsync(
                        context.Update.CallbackQuery.Message.Chat,
                        "Try again",
                        ParseMode.Markdown,
                        replyMarkup: new ReplyKeyboardRemove()
                    );
                    break;
                default:
                    break;
            };
        }

        private ReplyKeyboardMarkup CreateTreeMarkup() =>
                new ReplyKeyboardMarkup(new List<KeyboardButton>()
                    {
                        new KeyboardButton("My location"){ RequestLocation = true },
                    }
                )
                { ResizeKeyboard = true }
            ;
    }
}