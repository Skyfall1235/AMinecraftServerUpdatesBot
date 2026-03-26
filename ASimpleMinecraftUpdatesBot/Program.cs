using ASimpleMinecraftUpdatesBot.Services;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton(new DiscordSocketConfig
{
    GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildMessages,
    AlwaysDownloadUsers = true
});

builder.Services.AddSingleton<DiscordSocketClient>();
builder.Services.AddSingleton(provider =>
{
    var client = provider.GetRequiredService<DiscordSocketClient>();
    return new InteractionService(client);
});
builder.Services.AddSingleton<JsonService>();
builder.Services.AddSingleton<ConfigService>();
builder.Services.AddSingleton<MinecraftService>();

using IHost host = builder.Build();

var client = host.Services.GetRequiredService<DiscordSocketClient>();
var interactionService = host.Services.GetRequiredService<InteractionService>();

client.Ready += async () =>
{
    // 1. Find the first server the bot is in
    var guild = client.Guilds.FirstOrDefault();

    if (guild != null)
    {
        await interactionService.AddModulesAsync(typeof(Program).Assembly, host.Services);
        await interactionService.RegisterCommandsToGuildAsync(guild.Id);

        Console.WriteLine($"✅ Connected to {guild.Name} ({guild.Id}) and registered commands!");
    }
    else
    {
        Console.WriteLine("⚠️ The bot isn't in any servers yet! Invite it to your server first.");
    }
};

// Route incoming slash commands to the InteractionService
client.SlashCommandExecuted += async (interaction) =>
{
    var ctx = new SocketInteractionContext(client, interaction);
    await interactionService.ExecuteCommandAsync(ctx, host.Services);
};

// Login and Start
await client.LoginAsync(Discord.TokenType.Bot, "YOUR_BOT_TOKEN");
await client.StartAsync();

await host.RunAsync();