using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Bop_Bot
{
    class Program
    {
        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        //Declearing fields
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        public async Task RunBotAsync()
        {
            //estansiate new delceared fields
            _client = new DiscordSocketClient();
            _commands = new CommandService();

            _services = new ServiceCollection() //Adding singletons, because we need to have only 1 client and etc...
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();

            string botToken = "NDU1MzEzNDY1NzE2NzY4NzY5.Df6dgw.Q-rbRhwhFw-qXnYZ13j97krbJyo";  //bots hardcoded Token, dont reveal this token to anyone!!!!

            //event Subscriptions
            _client.Log += log;

            await _client.LoginAsync(TokenType.Bot, botToken);

            await _client.StartAsync();

            await Task.Delay(-1);
        
        }
        //made for event subsctiptions
        private Task log(LogMessage arg)
        {
            Console.WriteLine(arg);

            return Task.CompletedTask;
        }
        //
        public async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
           
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly()); // Will wait and find Modules if called by user, not sure tho...

        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;

            if (message is null || message.Author.IsBot) return; //If the authot is a bot or is null then it will return thetask

            int argPos = 0;

            if (message.HasStringPrefix("mikk!", ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var context = new SocketCommandContext(_client, message);

                var result = await _commands.ExecuteAsync(context, argPos, _services);

                if (!result.IsSuccess) // this will be false if its isnt a succsess, it is inverted.
                    Console.WriteLine(result.ErrorReason);

            }
        }
    }
}
