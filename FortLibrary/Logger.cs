﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortLibrary
{
    public static class Logger
    {
        private static StreamWriter writer;

        static Logger()
        {
            InitializeLogger();
        }

        private static void InitializeLogger()
        {
            try
            {
                writer = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FortBackend.log"), false) { AutoFlush = true };
                writer.WriteLine("FortBackend Logs");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to open log file: {ex.Message}");
            }
        }

        public static void PlainLog(string Message)
        {
            writer.WriteLine(Message);
            Console.WriteLine(Message);
        }

        public static void Log(string Message, string Custom = "Log")
        {
            writer.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} {Custom}] " + Message);
            Console.WriteLine($"\u001B[32m[{Custom}]: {Message}\u001B[0m");
        }

        public static void Warn(string Message)
        {
            writer.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} Warn] " + Message);
            Console.WriteLine($"\u001B[33m[Warn]: {Message}\u001B[0m");
        }

        public static void Error(string Message, string Custom = "Error")
        {
            writer.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} {Custom}] " + Message);
            Console.WriteLine($"\u001B[31m[{Custom}]: {Message}\u001B[0m");
        }
    }
}
