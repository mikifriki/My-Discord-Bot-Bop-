using System;
using System.Collections.Generic;
using System.Text;
using Discord.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace BopBot.Modules
{
    public class Ping : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task PingAsync()
        {
            await ReplyAsync("Asuh!");
        }
    }
}
