﻿using Discord;
using FortBackend.src.App.Utilities.Classes.EpicResponses.Content;
using FortBackend.src.App.Utilities.MongoDB.Helpers;
using FortBackend.src.App.Utilities.MongoDB.Module;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Numerics;

namespace FortBackend.src.App.Routes.APIS.API
{
    [ApiController]
    [Route("api")]
    public class ApisController : ControllerBase
    {
        [HttpPost("/datarouter/api/v1/public/data")]
        public async Task<IActionResult> DataRouter()
        {
            var queryParameters = HttpContext.Request.Query;
            Console.WriteLine("Query Parameters:");
            foreach (var (key, value) in queryParameters)
            {
                Console.WriteLine($"{key}: {value}");
            }
            var headers = HttpContext.Request.Headers;
            Console.WriteLine("\nHeaders:");
            foreach (var (key, value) in headers)
            {
                Console.WriteLine($"{key}: {value}");
            }

            using (var reader = new StreamReader(HttpContext.Request.Body))
            {
                try
                {
                    var requestBody = await reader.ReadToEndAsync();
                    Console.WriteLine($"Request Body: {requestBody}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading request body: {ex.Message}");
                }
            }
            return Ok();
        }

        //statsproxy/api/statsv2/account/
        [HttpGet("/statsproxy/api/statsv2/account/{accountId}")]
        public IActionResult StatsProxy(string accountId)
        {
            return Ok(new
            {
                startTime = 0,
                endTime = 9223372036854776000,
                accountId = accountId,
                stats = new { } // stats like "smth": number
            });
        }

        ///api/v1/events/Fortnite/download/644812f9-5e5e-4fd4-a670-b306e5956fd9
        [HttpGet("v1/events/Fortnite/download/{accountId}")]
        public IActionResult DownloadEndpoint(string accountId)
        {
            return Ok(new
            {
                player = new
                {
                    gameId = "Fortnite",
                    accountId = accountId,
                    token = new List<string>()
                    {
                        "ARENA_S15_Division1"
                    },
                    teams = new { },
                    pendingPayouts = new List<string>(),
                    pendingPenalties = new { },
                    persistentScores = new { },
                    groupIdentity = new { },
                },
                events = new List<object>()
                {
                    new
                    {
                        gameId = "Fortnite",
                        eventId = "Playlist_ShowdownAlt_Solo",
                        playlistId = "Playlist_ShowdownAlt_Solo",
                        regions = new string[] { "EU", "NAE" },
                        regionMapping = new { },
                        platforms = new string[]
                        {
                            "Windows"
                        },
                        platformMappings = new { },
                        displayDateId = "Playlist_ShowdownAlt_Solo",
                        eventGroup = "Playlist_ShowdownAlt_Solo",
                        announcementTime = "2019-01-29T08:00:00.000Z",
                        appId = "",
                        environment = "",
                        link = "",
                        metadata = new
                        {
                            AccountLockType = "Window",
                            TeamLockType = "None",
                            DisqualityType = "Window",
                            minimumAccountLevel = 0
                        },
                        beginTime = "2000-00-00T00:00:00.000Z",
                        endTime = "9999-00-00T00:00:000Z"

                    }
                },
                templates = new List<object>(),
                leaderboardDefs = new List<object>(),
                scoringRuleSets = new { },
                payoutTables = new { },
                scores = new { }
            });
        }

        //api/v1/user/setting

        [HttpPost("v1/user/setting")]
        public async Task<IActionResult> SettingsUser()
        {
            Response.ContentType = "application/json";
            try
            {
                var FormRequest = HttpContext.Request.Form;

                string accountId = "";

                if (FormRequest.TryGetValue("accountId", out var accountId1))
                {
                    accountId = accountId1;
                }


                Console.WriteLine(accountId);
                var AccountData = await Handlers.FindOne<Account>("accountId", accountId);
                if (AccountData != "Error")
                {
                    Account AccountDataParsed = JsonConvert.DeserializeObject<Account[]>(AccountData)?[0];
                    if(AccountDataParsed != null)
                    {
                        //return Ok(new List<object>() {
                        //    new
                        //    {
                        //        accountId = accountId,
                        //        key = "avatar",
                        //        value = $"{AccountDataParsed.athena.Items[AccountDataParsed.athena.last_applied_loadout]["attributes"]["locker_slots_data"]["slots"]["character"]["items"][0]}"
                        //    },
                        //    new {
                        //        accountId = accountId,
                        //        key = "avatarBackground",
                        //        value = "[\"#B4F2FE\",\"#00ACF2\",\"#005679\"]" // TEMP DON't WRRORY!
                        //    }
                        //});
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Ok(Array.Empty<string>());
        }

        [HttpGet("/waitingroom/api/waitingroom")]
        public IActionResult WaitingRoom()
        {
            return StatusCode(204);
        }

        [HttpGet("/eulatracking/api/public/agreements/fn/account/{accountId}")]
        public IActionResult eulatracking(string accountId)
        {
            return StatusCode(204);
        }

        //catalog/api/shared/bulk/offers

        [HttpGet("/catalog/api/shared/bulk/offers")]
        public IActionResult Catoffers()
        {
            Response.ContentType = "application/json";
            return Content("{}");
        }

        // /api/v1/assets/Fortnite/++Fortnite+Release-15.50/15526472?

        [HttpPost("/v1/assets/Fortnite/{version}/{number}")]
        public IActionResult AssetsV1(string version, string number)
        {
            Response.ContentType = "application/json";
            return Ok(new
            {
                FortPlaylistAthena = new
                {
                    meta = new
                    {
                        promotion = 0
                    },
                    assets = new { }
                }
            });
        }
    }
}
