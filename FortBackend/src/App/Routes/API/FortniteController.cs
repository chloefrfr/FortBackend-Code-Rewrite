﻿using FortBackend.src.App.Utilities.MongoDB.Helpers;
using FortBackend.src.App.Utilities.MongoDB.Module;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace FortBackend.src.App.Routes.API
{
    [ApiController]
    [Route("fortnite/api")]
    public class FortniteApiController : ControllerBase
    {
        [HttpGet("receipts/v1/account/{accountId}/receipts")]
        public IActionResult Receipts(string accountId)
        {
            return Ok(new List<object>()
            {
                new
                {
                  appStore = "EpicPurchasingService",
                  appStoreId = "69",
                  receiptId = "69",
                  receiptInfo = "ENTITLEMENT"
                }
            });
        }

        [HttpGet("versioncheck")]
        public IActionResult VersionCheck()
        {
            Response.ContentType = "application/json";

            return Ok(new
            {
                type = "NO_UPDATE"
            });
        }


        [HttpGet("v2/versioncheck/{version}")]
        public IActionResult VersionCheckV2(string version)
        {
            Response.ContentType = "application/json";

            return Ok(new
            {
                type = "NO_UPDATE"
            });
        }

        [HttpGet("game/v2/enabled_features")]
        public IActionResult enabled_features()
        {
            Response.ContentType = "application/json";
            return Ok(Array.Empty<string>());
        }

        [HttpGet("storeaccess/v1/request_access/{accountId}")]
        public IActionResult request_access()
        {
            Response.ContentType = "application/json";
            return Ok();
        }

        [HttpGet("game/v2/world/info")]
        public IActionResult WorldInfo()
        {
            return Ok(new { });
        }

        [HttpGet("game/v2/privacy/account/{accountId}")]
        public IActionResult PrivacyAcc(string accountId)
        {
            Response.ContentType = "application/json";

            return Ok(new
            {
                accountId,
                optOutOfPublicLeaderboards = false
            });
        }

        ///fortnite/api/game/v2/tryPlayOnPlatform/account/ <summary>
        /// fortnite/api/game/v2/tryPlayOnPlatform/account/

        [HttpPost("game/v2/tryPlayOnPlatform/account/{accountId}")]
        public IActionResult TryPlayOnPlatform(string accountId)
        {
            Response.ContentType = "text/plain";

            return Content("true");
        }

        //socialban/api/public/v1/
        [HttpGet("/socialban/api/public/v1/{accountId}")]
        public async Task<IActionResult> SocialBan(string accountId)
        {
            Response.ContentType = "application/json";
            return Ok(new
            {
                bans = Array.Empty<string>(),
                warnings = Array.Empty<string>(),
            });
        }

        // gold
        [HttpGet("game/v2/br-inventory/account/{accountId}")]
        public async Task<IActionResult> Accinventory(string accountId)
        {
            Response.ContentType = "application/json";
            var AccountData = await Handlers.FindOne<Account>("accountId", accountId);
            int globalcash = 0;
            if (AccountData != "Error")
            {
                Account AccountDataParsed = JsonConvert.DeserializeObject<Account[]>(AccountData)?[0];
                if (AccountDataParsed != null)
                {
                    globalcash = AccountDataParsed.athena.Gold;
                }
            }

            return Ok(new
            {
                stash = new
                {
                    globalcash,
                }
            });
        }
    }
}
