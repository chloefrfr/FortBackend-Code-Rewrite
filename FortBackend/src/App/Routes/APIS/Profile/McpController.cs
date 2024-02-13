﻿using FortBackend.src.App.Utilities.MongoDB.Helpers;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using FortBackend.src.App.Utilities.MongoDB.Module;
using FortBackend.src.App.Utilities;
using Newtonsoft.Json;
using FortBackend.src.App.Utilities.Helpers;
using FortBackend.src.App.Utilities.Classes.EpicResponses.Profile;
using FortBackend.src.App.Routes.APIS.Profile.McpControllers;

namespace FortBackend.src.App.Routes.APIS.Profile
{
    [ApiController]
    [Route("fortnite/api/game/v2/profile")]
    public class QueryProfileApiController : ControllerBase
    {

        // [HttpPost("{accountId}/{wildcard}/BulkEquipBattleRoyaleCustomization")]
        // [HttpPost("{accountId}/{wildcard}/ClientQuestLogin")] // temp 
        // [HttpPost("{accountId}/{wildcard}/QueryProfile")]
        [HttpPost("{accountId}/{wildcard}/{mcp}")]
        public async Task<ActionResult<Mcp>> McpApi(string accountId, string wildcard, string mcp)
        {
            Response.ContentType = "application/json";
            try
            {
                var RVN = int.Parse(Request.Query["rvn"].FirstOrDefault() ?? "-1");
                var ProfileID = Request.Query["profileId"].ToString() ?? "athena";
                Console.WriteLine("T");
                var AccountData = await Handlers.FindOne<Account>("accountId", accountId);
                Console.WriteLine("T");
                if (AccountData != "Error")
                {
                    Account AccountDataParsed = JsonConvert.DeserializeObject<Account[]>(AccountData)?[0];

                    var response = new Mcp();
                    if (AccountDataParsed != null)
                    {
                        var Season = await Grabber.SeasonUserAgent(Request);
                        switch (mcp)
                        {
                            case "QueryProfile":
                                response = await QueryProfile.Init(accountId, ProfileID, Season, RVN, AccountDataParsed);
                            break;
                            case "ClientQuestLogin":
                                response = await ClientQuestLogin.Init(accountId, ProfileID, Season, RVN, AccountDataParsed);
                            break;
                            default:
                            response = new Mcp
                            {
                                profileRevision = RVN,
                                profileId = ProfileID,
                                profileChangesBaseRevision = RVN,
                                //profileChanges = /,
                                profileCommandRevision = RVN,
                                serverTime = DateTime.Parse(DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")),
                                responseVersion = 1
                            };
                            break;
                        }

                        return response;
                    }
                }

                return Ok(new Mcp
                {
                    profileRevision = RVN,
                    profileId = ProfileID,
                    profileChangesBaseRevision = RVN,
                    //profileChanges = new object[0],
                    profileCommandRevision = RVN,
                    serverTime = DateTime.Parse(DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")),
                    responseVersion = 1
                });
            }
            catch (Exception ex)
            {
                Logger.Error($"QueryProfile: {ex.Message}");
            }

            return Ok(new Mcp
            {
                profileRevision = 1,
                profileId = "athena",
                profileChangesBaseRevision = 1,
                //profileChanges = new object[0],
                profileCommandRevision = 1,
                serverTime = DateTime.Parse(DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")),
                responseVersion = 1
            });
        }
    }
}
