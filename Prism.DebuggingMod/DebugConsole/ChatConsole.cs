﻿using System;
using System.Collections.Generic;
using System.Linq;
using Prism.DebuggingMod.ChatConsole.ChatCommands;
using Prism.Util;
using Terraria;
using Microsoft.Xna.Framework;

namespace Prism.DebuggingMod.ChatConsole
{
    public class ChatConsole
    {
        internal static Dictionary<string, ChatCommand> Commands = new List<ChatCommand>()
        {
            new ChatCommandCS (),
            new ChatCommandGet(),
            new ChatCommandSet(),
            new ChatCommandSpawn(),
            new ChatCommandHelp(),
        }
        .Select(x => new KeyValuePair<string, ChatCommand>(x.Name.ToLower(), x)).ToDictionary();

        public static void Error(string format, params object[] args)
        {
            Main.NewText(string.Format(format, args), 255, 0, 0, true);
        }

        public static void Info(string text)
        {
            Main.NewTextMultiline(text, true, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoG));
        }

        public static List<string> ParseArgs(params string[] args)
        {
            var resultList = new List<string>();
            string stringLiteral = null;

            for (int i = 0; i < args.Length; i++)
            {
                if (stringLiteral == null)
                {
                    if (args[i].StartsWith("'"))
                    {
                        stringLiteral = args[i];
                    }
                    else
                    {
                        resultList.Add(args[i]);
                    }
                }
                else
                {
                    stringLiteral += " " + args[i];
                    if (args[i].EndsWith("'"))
                    {
                        resultList.Add(stringLiteral.Substring(1, stringLiteral.Length - 2));
                        stringLiteral = null;
                    }
                }
            }

            if (stringLiteral != null)
            {
                resultList.Add(stringLiteral.Substring(1, stringLiteral.Length - 1));
                stringLiteral = null;
            }

            return resultList;
        }

        public static bool RunCmd(string text)
        {
            var splitArgs = ParseArgs(text.Trim().Split(' '));
            if (splitArgs[0].StartsWith("/"))
            {
                // The command name will be everything after the first forward slash until the first space.
                var cmdName = splitArgs[0].Substring(1);
                // The args will be everything after the first space.
                var args = text.Substring(text.IndexOf(' ') + 1);
                if (Commands.ContainsKey(cmdName.ToLower()))
                {
                    if (!RunCmd(cmdName.ToLower(), args, GetSplitArgs(text)))
                    {
                        Main.NewText("The parameters '" + args + "' are invalid for command '" + cmdName.ToLower() + "'. Type '/help " + cmdName + "' for more info.", 255, 0, 0, true);
                        return false;
                    }
                }
                else
                {
                    Main.NewText("Command not found: '" + cmdName.ToLower() + "'.", 255, 0, 0, true);
                    return false;
                }
            }
            return true;
        }

        public static bool RunCmd(string cmd, string args)
        {
            return RunCmd(cmd, args, GetSplitArgs("/" + cmd + " " + args));
        }

        public static bool RunCmd(string cmd, List<string> splitArgs)
        {
            string args = "";
            for (int i = 0; i < splitArgs.Count; i++)
            {
                args += (i > 0 ? " " : "") + splitArgs[i];
            }
            return RunCmd(cmd, args, splitArgs);
        }

        public static bool RunCmd(string cmd, string args, List<string> splitArgs)
        {
            splitArgs = splitArgs ?? new List<string>();
            if (splitArgs.Count < Commands[cmd.ToLower()].MinArgs)
                return false;
            int i = 0;
            Commands[cmd.ToLower()].Run(Commands[cmd].CaseSensitive ? args : args.ToLowerInvariant(), splitArgs
                            .Select(x => Commands[cmd].CaseSensitive ? x : x.ToLowerInvariant())
                            .Where(x => (i++ < Commands[cmd].MaxArgs))
                            .ToList());
            return true;
        }

        public static List<string> GetSplitArgs(string text)
        {
            var splitArgs = ParseArgs(text.Trim().Split(' '));
            return splitArgs.ToArray().Reverse().Take(splitArgs.Count - 1).Reverse().ToList();//LOL
        }
    }
}
