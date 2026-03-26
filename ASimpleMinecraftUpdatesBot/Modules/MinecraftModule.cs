using ASimpleMinecraftUpdatesBot.Services;
using Discord.Interactions;
using Discord.WebSocket;

namespace ASimpleMinecraftUpdatesBot.Modules
{
    public class MinecraftModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly JsonService _jsonService;
        private readonly MinecraftService _mcService;


        public MinecraftModule(JsonService jsonService, MinecraftService mcService)
        {
            _jsonService = jsonService;
            _mcService = mcService;
        }

        [SlashCommand("status", "Get the current status of the Minecraft server.")]
        public async Task StatusCommandAsync()
        {
            await DeferAsync();
            var config = _jsonService.Config;
            var status = await _mcService.GetFullStatus(config);
            if (status.IsOnline)
            {
                await FollowupAsync($"✅ **Server Online!** Players: `{status.CurrentOverMaxPlayers}`");
            }
            else
            {
                await FollowupAsync("❌ **Server Offline.** Check the IP or port settings.");
            }
        }

        [SlashCommand("setup", "Configure the Minecraft server settings.")]
        public async Task SetupCommandAsync(
            [Summary("name", "The name of the server")] string serverName,
            [Summary("ip", "The IP address of the server")] string ip,
            [Summary("port", "The port (default is 25565)")] ushort port = 25565,
            [Summary("channel", "The channel for updates")] SocketTextChannel? channel = null)
        {
            await DeferAsync(ephemeral: true); // 'ephemeral' means only the admin sees this

            try
            {
                // 1. Use your ConfigService to update the JSON
                // We pass the channel ID if they provided a channel
                ConfigService.UpdateConfig(serverName, ip, port, channel?.Id);

                await FollowupAsync($"✅ **Settings Saved!**\nIP: `{ip}`\nPort: `{port}`\nChannel: {channel?.Mention ?? "None"}");
            }
            catch (Exception ex)
            {
                await FollowupAsync($"❌ **Error saving config:** {ex.Message}");
            }
        }
    }
}