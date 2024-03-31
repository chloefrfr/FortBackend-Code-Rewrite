﻿using Newtonsoft.Json;

namespace FortBackend.src.App.Utilities.Classes.ConfigHelpers
{
    // Helps Ini by not dynamic data
    public class IniConfig
    {
        [JsonProperty("//")]
        public string Info { get; set; } = "FORTBACKEND INI MANAGER";

        public List<IniConfigFiles> FileData { get; set; } = new List<IniConfigFiles>();

    }

    // File Name Data
    public class IniConfigFiles
    {
        
        public string Name { get; set; } = string.Empty;
        public List<IniConfigData> Data { get; set; } = new List<IniConfigData>();
    }

    public class IniConfigData
    {
        public string Title { get; set; } = string.Empty;
        public List<IniConfigValues> Data { get; set; } = new List<IniConfigValues>();
    }

    public class IniConfigValues
    {
        public string Name { get; set; } = string.Empty;
        public dynamic Value { get; set; } = string.Empty;
    }
}
