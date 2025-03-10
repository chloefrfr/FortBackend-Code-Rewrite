﻿namespace FortLibrary.EpicResponses.FortniteServices.Content
{
    public class BattleRoyaleNews
    {
        public NewContent news { get; set; } = new NewContent();
    };

    public class NewContent
    {
        public string _type { get; set; } = "Battle Royale News";
        public List<NewContentMotds> motds { get; set; } = new List<NewContentMotds>();
        public List<NewContentMessages> messages { get; set; } = new List<NewContentMessages>();
        public string _title { get; set; } = "battleroyalenews";
        public string header { get; set; } = "";
        public bool _noIndex { get; set; } = false;
        public bool alwaysShow { get; set; } = false;
        public string style { get; set; } = "SpecialEvent";
        public string _activeData { get; set; } = "2023-11-09T18:08:17.347Z";
        public string _locale { get; set; } = "en-US";
    };
    public class NewContentMotds
    {
        public string image { get; set; } = "https://cdn2.unrealengine.com/ch4s2-lobbyupdate-4-20-2022-lifted-copy-3840x2160-d3a138f5f9e7.jpg";
        public string tileImage { get; set; } = "https://cdn2.unrealengine.com/ch4s2-lobbyupdate-4-20-2022-lifted-copy-3840x2160-d3a138f5f9e7.jpg";
        public bool hidden { get; set; } = false;
        public string _type { get; set; } = "CommonUI Simple Message MOTD";
        public string entrytype { get; set; } = "Website"; // "normal"
        public string messagetype { get; set; } = "normal";
        public string tabTitleOverride { get; set; } = "FortBackend";
        public string title { get; set; } = "FortBackend";
        public string body { get; set; } = "Fortnite E-Kittens :3";
        public int sortingPriority { get; set; } = 0;
        public string id { get; set; } = "femboy69";
        public bool videoStreamingEnabled { get; set; } = false;
        public bool videoLoop { get; set; } = false;
        public bool videoMute { get; set; } = false;
        public bool videoAutoplay { get; set; } = false;
        public bool videoFullscreen { get; set; } = false;
        public bool spotlight { get; set; } = true;
        public string websiteURL { get; set; } = "https://github.com/zinx28/FortServer";
        public string websiteButtonText { get; set; } = "Github!!";
    }

    public class NewContentMessages
    {
        public string image { get; set; } = "https://cdn2.unrealengine.com/ch4s2-lobbyupdate-4-20-2022-lifted-copy-3840x2160-d3a138f5f9e7.jpg";
        public bool hidden { get; set; } = false;
        public string _type { get; set; } = "CommonUI Simple Message Base";
        public string entrytype { get; set; } = "normal";
        public string messagetype { get; set; } = "normal";
        public string title { get; set; } = "FortBackend";
        public string body { get; set; } = "Fortnite E-Kittens :3";
        public bool spotlight { get; set; } = false;
    }
}
