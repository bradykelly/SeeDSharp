using System;
using System.IO;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;

namespace SeeDSharp.Console
{
    class Program
    {
        private static DiscordClient discord;
        private static CommandsNextModule commands;
        private static InteractivityModule interactivity;

        static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = await File.ReadAllTextAsync(@"..\..\..\..\secrets\token"),
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug
            });

            discord.MessageCreated += async e =>
            {
                if (e.Message.Content.ToLower().StartsWith("ping"))
                    await e.Message.RespondAsync("pong!");
            };

            commands = discord.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefix = ";;"
            });
            commands.RegisterCommands<SimpleCommands>();

            interactivity = discord.UseInteractivity(new InteractivityConfiguration());

            await discord.ConnectAsync();
            System.Console.WriteLine("Connected to Discord");

            await Task.Delay(-1);
        }
    }
}
