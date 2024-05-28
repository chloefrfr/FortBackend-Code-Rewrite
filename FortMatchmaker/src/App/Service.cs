﻿using FortLibrary;
using FortLibrary.ConfigHelpers;
using FortMatchmaker.src.App.Utilities;
using FortMatchmaker.src.App.Utilities.Classes;
using FortMatchmaker.src.App.Utilities.Constants;
using FortMatchmaker.src.App.Utilities.MongoDB;
using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace FortMatchmaker.src.App
{
    public class Service
    {
        public static async void Intiliazation(string[] args)
        {
            Console.WriteLine(@"  ______         _   __  __       _       _                     _             
 |  ____|       | | |  \/  |     | |     | |                   | |            
 | |__ ___  _ __| |_| \  / | __ _| |_ ___| |__  _ __ ___   __ _| | _____ _ __ 
 |  __/ _ \| '__| __| |\/| |/ _` | __/ __| '_ \| '_ ` _ \ / _` | |/ / _ \ '__|
 | | | (_) | |  | |_| |  | | (_| | || (__| | | | | | | | | (_| |   <  __/ |   
 |_|  \___/|_|   \__|_|  |_|\__,_|\__\___|_| |_|_| |_| |_|\__,_|_|\_\___|_|   
                                                                              
                                                                              ");

            Logger.Log("MARVELCO MATCHMAKER IS LOADING (marcellowmellow)");
            Logger.Log($"Built on {RuntimeInformation.OSArchitecture}-bit");

            var builder = WebApplication.CreateBuilder(args);
            var startup = new Startup(builder.Configuration);

            var ReadConfig = File.ReadAllText(Path.Combine(PathConstants.BaseDir, "config.json"));
            
            if (ReadConfig == null)
            {
                Logger.Error("Couldn't find config (config.json)", "FortConfig");
                throw new Exception($"Couldn't find config\n{Path.Combine(PathConstants.BaseDir, "config.json")}");
            }

            Saved.DeserializeConfig = JsonConvert.DeserializeObject<FortConfig>(ReadConfig)!;
            
            if (Saved.DeserializeConfig == null)
            {
                Logger.Error("Couldn't deserialize config", "FortConfig");
                throw new Exception("Couldn't deserialize config");
            }
            else { 
                Logger.Log("Loaded Config", "FortConfig");
            }



            startup.ConfigureServices(builder.Services);

#if HTTPS
                Saved.BackendCachedData.DefaultProtocol = "https://";
                builder.WebHost.UseUrls($"https://0.0.0.0:{Saved.DeserializeConfig.MatchmakerPort}");
                builder.WebHost.ConfigureKestrel(serverOptions =>
                {
                    serverOptions.Listen(IPAddress.Any, Saved.DeserializeConfig.MatchmakerPort, listenOptions =>
                    {
                        var certPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "src", "Resources", "Certificates", "FortBackend.pfx");
                        if(!File.Exists(certPath)) {
                            Logger.Error("Couldn't find FortBackend.pfx -> make sure you removed .temp from FortBackend.pfx.temp", CERTIFICATES);
                            throw new Exception("Couldn't find FortBackend.pfx -> make sure you removed .temp from FortBackend.pfx.temp");
                        }
                        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                        var certificate = new X509Certificate2(certPath);
                        listenOptions.UseHttps(certificate);
                    });
                });
#else
            Saved.BackendCachedData.DefaultProtocol = "http://";
            builder.WebHost.UseUrls($"http://0.0.0.0:{Saved.DeserializeConfig.MatchmakerPort}");
            #endif


            var app = builder.Build();


           #if HTTPS
                app.UseHttpsRedirection();
            #endif

            app.UseWebSockets();

            startup.Configure(app, app.Environment);

            app.Run();

            //var shutdownTask = app.WaitForShutdownAsync();

            //AppDomain.CurrentDomain.ProcessExit += (sender, eventArgs) =>
            //{
            //    Logger.Error("Matchmaker Shuting Down!!");
            //};

            //await shutdownTask;
        }
    }
}
