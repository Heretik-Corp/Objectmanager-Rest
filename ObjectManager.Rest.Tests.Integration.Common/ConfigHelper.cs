using System;
using System.IO;

namespace ObjectManager.Rest.Tests.Integration.Common
{
    public static class ConfigHelper
    {
        private class Config
        {
            public string Url { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
        }
        private static Lazy<Config> Settings = new Lazy<Config>(GetSettings);

        private static Config GetSettings()
        {
            var text = File.ReadAllText(@"..\..\config.json");
            var value = Newtonsoft.Json.JsonConvert.DeserializeObject<Config>(text);
            return value;
        }
        public static string Url { get; } = Settings.Value.Url;
        public static string UserName { get; } = Settings.Value.UserName;
        public static string Password { get; } = Settings.Value.Password;
    }
}
