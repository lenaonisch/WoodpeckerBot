using System.Threading.Tasks;
using System.Collections.Generic;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace WoodpeckerBot.Handlers
{
    class PingCommand : CommandBase
    {
        public override async Task HandleAsync(IUpdateContext context, UpdateDelegate next, string[] args)
        {
            Message msg = context.Update.Message;

            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat,
                "🌳 Hello, I'm a woodpecker! 🌳\n\r" +
                "What would you like to do?",
                ParseMode.Markdown,
                replyToMessageId: msg.MessageId,
                replyMarkup: CreateMenu()   
            );
        }

        private InlineKeyboardMarkup CreateMenu() =>
            new InlineKeyboardMarkup(new List<InlineKeyboardButton>()
                    {
                        InlineKeyboardButton.WithCallbackData("Find a tree", "start/TREE"),
                        InlineKeyboardButton.WithCallbackData("Do a hole", "start/HOLE"),
                        InlineKeyboardButton.WithCallbackData("Count worms", "start/WORMS")
                    }
            );
    }
}