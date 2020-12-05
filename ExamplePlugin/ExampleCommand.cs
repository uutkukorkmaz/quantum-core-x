﻿using QuantumCore.API;
using QuantumCore.API.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExamplePlugin
{
    [Command("test", "This is an example command")]
    public class TestCommand
    {
        public static void Run(IPlayer player)
        {
            Console.WriteLine("it works!");
        }

        [CommandMethod("second desc")]
        public static void Run(IPlayer player, int required1, string optional1 = "")
        {
            Console.WriteLine($"Run with {required1} {optional1}");
        }

        [CommandMethod("third desc")]
        public static void Run(IPlayer player, float required1, string optional1 = "")
        {
            Console.WriteLine($"Run with {required1} and {optional1}");
        }
    }
}