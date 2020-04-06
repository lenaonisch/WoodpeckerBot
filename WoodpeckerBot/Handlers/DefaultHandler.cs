using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using IBWT.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace WoodpeckerBot.Handlers
{
    public class DefaultHandler : IUpdateHandler
    {
        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            Message msg = context.Update.Message ?? context.Update.CallbackQuery.Message;
            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat,
                //context.Items["History"].ToString() + " and last item = " +  context.Items["State"].ToString(),
                "🌳 Hello, I'm a woodpecker! 🌳\n\r" +
                "What would you like to do?",
                ParseMode.Markdown,
                replyToMessageId: msg.MessageId,
                /*replyMarkup: new InlineKeyboardMarkup(
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("menu1", "menu1::")
                    }*/
                replyMarkup: CreateMenu(),
                
                cancellationToken: cancellationToken
            );
        }

        private InlineKeyboardMarkup CreateMenu() =>
            new InlineKeyboardMarkup(new List<InlineKeyboardButton>()
                    {
                        InlineKeyboardButton.WithCallbackData("Find a tree", "menu1::TREE"),
                        InlineKeyboardButton.WithCallbackData("Do a hole", "menu1::HOLE"),
                        InlineKeyboardButton.WithCallbackData("Count worms", "menu1::WORMS")
                    }
            );
    }
}