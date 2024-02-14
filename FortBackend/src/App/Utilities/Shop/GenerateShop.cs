﻿using FortBackend.src.App.Utilities.Shop.Helpers;
using FortBackend.src.App.Utilities.Shop.Helpers.Data;
using SkiaSharp;
using System.Diagnostics;
using System;
using System.Drawing;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Components.Forms;
using MongoDB.Bson.Serialization.Serializers;

namespace FortBackend.src.App.Utilities.Shop
{
    public class GenerateShop
    {
        public static async Task Init()
        {
            var stopwatch = Stopwatch.StartNew();
            Logger.Log("Generating Shop", "ItemShop");
            SavedData savedData = new SavedData();

            savedData = await Generator.Start(savedData);
            var OutPutFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "src", "Resources", "output.png");
            int Width = 600;
            int Height = 500;
            using (var bitmap = new SKBitmap(Width, Height))
            {

                using (var canvas = new SKCanvas(bitmap))
                {
                    canvas.Clear(new SKColor(25, 25, 27));
                    //canvas.DrawRect(50, 50, 150, 150, new SKPaint { Color = SKColors.Blue });

                    canvas.DrawText("FortBackend ItemShop", 200, 50, new SKPaint
                    {
                        Color = SKColors.White,
                        TextSize = 25
                    });

                    canvas.DrawText("I'll work on this later", 200, 200, new SKPaint
                    {
                        Color = SKColors.White,
                        TextSize = 25
                    });


                    //canvas.DrawText("Daily", 50, 100, new SKPaint
                    //{
                    //    Color = SKColors.White,
                    //    TextSize = 20
                    //});

                    //if (savedData.Daily.Any())
                    //{
                    //    foreach (var shop in savedData.Daily)
                    //    {

                    //    }
                    //}else
                    //{
                    //    canvas.DrawText("There is no items in daily today!", 140, 190, new SKPaint
                    //    {
                    //        Color = SKColors.White,
                    //        TextSize = 26
                    //    });
                    //}

                    //canvas.DrawText("Weekly", 50, 300, new SKPaint
                    //{
                    //    Color = SKColors.White,
                    //    TextSize = 20
                    //});

                    //if (savedData.Weekly.Any())
                    //{
                    //    string WhatCate = "";
                    //    foreach (var shop in savedData.Weekly)
                    //    {
                    //        WhatCate = shop.categories
                    //        Console.WriteLine(shop.name);
                    //    }
                    //}
                    //else
                    //{
                    //    canvas.DrawText("There is no items in weekly today!", 140, 190, new SKPaint
                    //    {
                    //        Color = SKColors.White,
                    //        TextSize = 26
                    //    });
                    //}

                    //var Boxes = new List<SKRectI>();
                    //var Random = new Random();
                    //var CurrentBoxLeft = 0;
                    //var CurrentBoxTop = 0;
                    //foreach (var shop in savedData.Daily)
                    //{
                    //    if (shop.name.Contains("Bundle"))
                    //    {
                    //        Console.WriteLine("BUNDLE");
                    //    }
                    //}

                    //foreach (var shop in savedData.Weekly)
                    //{
                    //    if (shop.name.Contains("Bundle"))
                    //    {
                    //        Console.WriteLine("BUNDLE");
                    //    }
                    //    else
                    //    {
                    //        int Left = 300 + CurrentBoxLeft;
                    //        int Top = 30 + CurrentBoxTop;
                    //        int Right = Left + CurrentBoxLeft + 50;
                    //        int Bottom = Top + (CurrentBoxTop - 50);


                    //        canvas.DrawRect(new SKRect(Left, Top, Right, Bottom), new SKPaint
                    //        {
                    //            Color = SKColors.White,
                    //            Style = SKPaintStyle.Fill
                    //        });
                    //    }
                    //}


                }

                using (var image = SKImage.FromBitmap(bitmap))
                using (var data = image.Encode(SKEncodedImageFormat.Png, 1000))
                using (var stream = System.IO.File.OpenWrite(OutPutFile))
                {
                    Console.WriteLine("SAVED DATA");
                    data.SaveTo(stream);
                }

            }
            stopwatch.Stop();
            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            Logger.Log($"Shop Generated in {elapsedMilliseconds}ms", "ItemShop");

            //await DiscordWebsocket.SendEmbed();
        }
    }
}
