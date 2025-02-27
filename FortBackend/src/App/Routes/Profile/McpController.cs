using FortBackend.src.App.Utilities.MongoDB.Helpers;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using FortBackend.src.App.Utilities;
using Newtonsoft.Json;
using System.Text;
using static FortBackend.src.App.Utilities.Helpers.Grabber;
using FortBackend.src.App.Routes.Profile.McpControllers;
using FortBackend.src.App.Utilities.Helpers.Middleware;
using System.IdentityModel.Tokens.Jwt;
using FortLibrary.EpicResponses.Errors;
using FortLibrary.EpicResponses.Profile;
using FortLibrary.EpicResponses.Profile.Purchases;
using FortLibrary;
using FortBackend.src.XMPP.Data;
using FortLibrary.Encoders.JWTCLASS;

namespace FortBackend.src.App.Routes.Profile
{
    [ApiController]
    [Route("fortnite/api/game/v2/profile")]
    public class QueryProfileApiController : ControllerBase
    {
        [HttpPost("{accountId}/{wildcard}/{mcp}")]
        [AuthorizeToken]
        public async Task<ActionResult<Mcp>> McpApi(string accountId, string wildcard, string mcp)
        {
            Response.ContentType = "application/json";
            try
            {
                var rvn = int.TryParse(Request.Query["rvn"].FirstOrDefault(), out var parsedRvn) ? parsedRvn : -1;
                var profileId = Request.Query["profileId"].ToString() ?? "athena";
                
                using var reader = new StreamReader(Request.Body, Encoding.UTF8);
                var requestBody = await reader.ReadToEndAsync();
                
                if (string.IsNullOrEmpty(requestBody))
                {
                    return CreateErrorResponse("No Body for request", accountId, wildcard, mcp);
                }
                
                var season = await SeasonUserAgent(Request);
                var profileCacheEntry = await GetProfileCacheEntry(accountId, wildcard);
                if (profileCacheEntry == null)
                {
                    return CreateErrorResponse("Authentication failed", accountId, wildcard, mcp);
                }
                
                var response = await HandleMcpRequest(mcp, accountId, profileId, season, rvn, profileCacheEntry, requestBody);
                return response;
            }
            catch (BaseError ex)
            {
                return StatusCode(500, JsonConvert.SerializeObject(BaseError.FromBaseError(ex)));
            }
            catch (Exception ex)
            {
                Logger.Error($"McpController: {ex.Message}", "MCP");
                return Ok(DefaultMcpResponse());
            }
        }

        private async Task<ProfileCacheEntry?> GetProfileCacheEntry(string accountId, string wildcard)
        {
            if (wildcard == "dedicated_server")
            {
                return await GrabData.Profile(accountId);
            }
            
            var tokenPayload = HttpContext.Items["Payload"] as TokenPayload;
            if (string.IsNullOrEmpty(tokenPayload?.Sec)) return null;
            
            return HttpContext.Items["ProfileData"] as ProfileCacheEntry;
        }

        private async Task<Mcp> HandleMcpRequest(string operatiom, string accountId, string profileId, VersionClass season, int rvn, ProfileCacheEntry profileCacheEntry, string requestBody)
        {
            Logger.Log($"Operation '{operatiom}' called!", "MCP");
            return operatiom switch
            {
                "QueryProfile" => await QueryProfile.Init(accountId, profileId, season, rvn, profileCacheEntry),
                "ClientQuestLogin" => await ClientQuestLogin.Init(accountId, profileId, season, rvn, profileCacheEntry),
                "SetCosmeticLockerSlot" => await SetCosmeticLockerSlot.Init(accountId, profileId, season, rvn, profileCacheEntry, JsonConvert.DeserializeObject<SetCosmeticLockerSlotRequest>(requestBody)!),
                "EquipBattleRoyaleCustomization" => await EquipBattleRoyaleCustomization.Init(accountId, profileId, season, rvn, profileCacheEntry, JsonConvert.DeserializeObject<EquipBattleRoyaleCustomizationRequest>(requestBody)!),
                "BulkEquipBattleRoyaleCustomization" => await BulkEquipBattleRoyaleCustomization.Init(accountId, profileId, season, rvn, profileCacheEntry, JsonConvert.DeserializeObject<BulkEquipBattleRoyaleCustomizationResponse>(requestBody)!),
                "MarkNewQuestNotificationSent" => await MarkNewQuestNotificationSent.Init(accountId, profileId, season, rvn, profileCacheEntry, JsonConvert.DeserializeObject<MarkNewQuestNotificationSentRequest>(requestBody)!),
                "MarkItemSeen" => await MarkItemSeen.Init(accountId, profileId, season, rvn, profileCacheEntry, JsonConvert.DeserializeObject<MarkNewQuestNotificationSentRequest>(requestBody)!),
                "SetPartyAssistQuest" => await SetPartyAssistQuest.Init(accountId, profileId, season, rvn, profileCacheEntry, JsonConvert.DeserializeObject<SetPartyAssistQuestResponse>(requestBody)!),
                "SetBattleRoyaleBanner" => await SetBattleRoyaleBanner.Init(accountId, profileId, season, rvn, profileCacheEntry, JsonConvert.DeserializeObject<SetBattleRoyaleBannerReq>(requestBody)!),
                "PurchaseCatalogEntry" => await PurchaseCatalogEntry.Init(accountId, profileId, season, rvn, profileCacheEntry, JsonConvert.DeserializeObject<PurchaseCatalogEntryRequest>(requestBody)!),
                "RemoveGiftBox" => await RemoveGiftBox.Init(accountId, profileId, season, rvn, profileCacheEntry, JsonConvert.DeserializeObject<RemoveGiftBoxReq>(requestBody)!),
                "FortRerollDailyQuest" => await FortRerollDailyQuest.Init(accountId, profileId, season, rvn, profileCacheEntry, JsonConvert.DeserializeObject<FortRerollDailyQuestReq>(requestBody)!),
                "UpdateQuestClientObjectives" => await UpdateQuestClientObjectives.Init(accountId, profileId, season, rvn, profileCacheEntry, JsonConvert.DeserializeObject<UpdateQuestClientObjectivesReq>(requestBody)!),               
                _ => DefaultMcpResponse(profileId, profileCacheEntry)
            };
        }

        private Mcp DefaultMcpResponse(string profileId = "athena", ProfileCacheEntry? profileCacheEntry = null)
        {
            var rvn = profileCacheEntry?.AccountData.athena.RVN ?? 1;
            return new Mcp
            {
                profileRevision = rvn,
                profileId = profileId,
                profileChangesBaseRevision = rvn,
                profileCommandRevision = profileCacheEntry?.AccountData.athena.CommandRevision ?? 1,
                serverTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                responseVersion = 1
            };
        }

        private ActionResult CreateErrorResponse(string message, string accountId, string wildcard, string mcp)
        {
            var error = new BaseError
            {
                errorCode = "errors.com.epicgames.common.authentication.authentication_failed",
                errorMessage = $"{message} for /api/game/v2/profile/{accountId}/{wildcard}/{mcp}",
                messageVars = new List<string> { $"/api/game/v2/profile/{accountId}/{wildcard}/{mcp}" },
                numericErrorCode = 1032,
                originatingService = "any",
                intent = "prod",
                error_description = $"{message} for /api/game/v2/profile/{accountId}/{wildcard}/{mcp}"
            };
            return StatusCode(500, JsonConvert.SerializeObject(BaseError.FromBaseError(error)));
        }
    }
}