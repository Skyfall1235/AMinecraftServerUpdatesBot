using System.Text.Json;

namespace ASimpleMinecraftUpdatesBot.Services
{
    public class BotConfig
    {
        public string ServerName { get; set; } = "";
        public string MinecraftIp { get; set; } = "127.0.0.1";
        public ushort Port { get; set; } = 25565;
        public ulong TargetChannelId { get; set; }
    }

    public class ConfigService
    {
        static JsonService? jsonService;

        public ConfigService(JsonService service)
        {
            jsonService = service;
        }

        public static void UpdateConfig(string? name = "", string? ip = null, ushort? port = null, ulong? channelId = null)
        {
            var currentConfig = jsonService!.Config;
            if (!string.IsNullOrEmpty(name)) currentConfig.ServerName = name;
            if (!string.IsNullOrEmpty(ip)) currentConfig.MinecraftIp = ip;
            if (port.HasValue) currentConfig.Port = port.Value;
            if (channelId.HasValue) currentConfig.TargetChannelId = channelId.Value;

            jsonService.SaveConfig(currentConfig);
        }

        public static void SaveToFile()
        {
            jsonService!.SaveTextToFile();
        }


    }

    public class JsonService
    {
        private readonly string _path = "data/config.json";
        public BotConfig Config { get; private set; }

        public JsonService()
        {
            if (!Directory.Exists("data")) Directory.CreateDirectory("data");
            Config = Load();
        }

        public BotConfig Load()
        {
            if (!File.Exists(_path)) return new BotConfig();
            var json = File.ReadAllText(_path);
            return JsonSerializer.Deserialize<BotConfig>(json) ?? new BotConfig();
        }

        public void SaveConfig(BotConfig newConfig)
        {
            Config = newConfig;
            SaveTextToFile();
        }

        public void SaveTextToFile() => File.WriteAllText(_path, JsonSerializer.Serialize(Config, new JsonSerializerOptions { WriteIndented = true }));
    }
}
