using MineStatLib;

namespace ASimpleMinecraftUpdatesBot.Services
{
    public class MinecraftService
    {
        public async Task<bool> GetAliveStatus(BotConfig config)
        {
            MineStat ms = await getMineStat(config.MinecraftIp, config.Port);
            return ms.ServerUp;
        }


        public async Task<MCServerStatus> GetFullStatus(BotConfig config)
        {
            MineStat ms = await getMineStat(config.MinecraftIp, config.Port);
            if(!ms.ServerUp) return new MCServerStatus();

            //if it doesnt fail to find the server, go on
            string players = $"{ms.CurrentPlayers}/{ms.MaximumPlayers}";
            MCServerStatus status = new MCServerStatus(players);
            return status;
        }

        private async Task<MineStat> getMineStat(string ip, ushort port)
        {
            MineStat ms = await Task.Run(() => new MineStat(ip, port));
            return ms;
        }
        
    }

    public struct MCServerStatus
    {
        public MCServerStatus(string currentOverMaxPlayers)
        {
            IsOnline = true;
            CurrentOverMaxPlayers = currentOverMaxPlayers;
        }

        public bool IsOnline { get; set; }
        public string CurrentOverMaxPlayers { get; set; }
    }
}
