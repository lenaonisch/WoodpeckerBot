using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Telegram.Bot.Framework;
using Telegram.Bot.Types;

namespace Quickstart.AspNetCore
{
    public class EchoBot : BotBase
    {
        public EchoBot(IOptions<BotOptions<EchoBot>> options)
            : base(options.Value)
        {
        }
    }
}
