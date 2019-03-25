// Copyright (C) 2019 Sebastian Lühnen
//
//
// This file is part of NerdyAion.
// 
// NerdyAion is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// NerdyAion is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with NerdyAion. If not, see <http://www.gnu.org/licenses/>.
//
//
// Created By: Sebastian Lühnen
// Created On: 19.02.2019
// Last Edited On: 25.02.2019
// Language: C#
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NerdyAion
{
    public static class Commands
    {
        private static String activCommantLevel;
        private static Dictionary<String, LogAnalyzer> analyzer;
        private static SettingsController settings;

        public static String ActivCommantLevel
        {
            get { return activCommantLevel; }
            set { activCommantLevel = value; }
        }
        public static Dictionary<String, LogAnalyzer> Analyzer
        {
            get { return analyzer; }
            set { analyzer = value; }
        }
        public static SettingsController Settings
        {
            get { return settings; }
            set { settings = value; }
        }
        
        public static void Execut(String command)
        {
            String[] commandParts = command.Split(' ');

            if (commandParts.Length < 1)
            {
                commandParts = new String[1];
                commandParts[0] = command;
            }

            switch (ActivCommantLevel)
            {
                case CommantLevel.BaseLevel:
                    Commands.BaseCommands(commandParts, command);
                    break;
                case CommantLevel.SettingsLevel:
                    Commands.SettingsCommands(commandParts, command);
                    break;
                case CommantLevel.DmgLevel:
                    Commands.DmgCommands(commandParts, command);
                    break;
                default:
                    Commands.BaseCommands(commandParts, command);
                    break;
            }
        }

        private static void BaseCommands(String[] command, String usedCommand)
        {
            switch (command[0])
            {
                case "info":
                    Console.WriteLine();
                    Console.WriteLine("###################################################################");
                    Console.WriteLine("NerdyAion [" + Settings.GetSetting("version") + "]");
                    Console.WriteLine("Author: SCHREDDO");
                    Console.WriteLine("Repository: https://github.com/SCHREDDO/NerdyAion-Aion-Tool-Manager");
                    Console.WriteLine("###################################################################");
                    Console.WriteLine();
                    break;
                case "chatlog":
                    if (command.Length < 2)
                    {
                        Commands.ShowError("unknown command \"" + usedCommand + "\"");
                    }
                    else
                    {
                        if (command[1] == "on")
                        {
                            SystemCFGEditor.SetChatLogActive(Settings.GetSetting("aion") + @"\system.cfg");
                        }
                        else
                        {
                            Commands.ShowError("unknown command \"" + usedCommand + "\"");
                        }
                    }
                    break;
                case "clear":
                    Console.Clear();
                    break;
                case "goto":
                    if (command.Length < 2)
                    {
                        Commands.ShowError("unknown command \"" + usedCommand + "\"");
                    }
                    else
                    {
                        switch (command[1])
                        {
                            case "dmg":
                                ActivCommantLevel = CommantLevel.DmgLevel;
                                break;
                            case "settings":
                                ActivCommantLevel = CommantLevel.SettingsLevel;
                                break;
                            default:
                                Commands.ShowError("unknown path \"" + command[1] + "\"");
                                break;
                        }
                    }
                    break;
                case "back":
                    ActivCommantLevel = CommantLevel.BaseLevel;
                    break;
                case "help":
                    if (command.Length < 2)
                    {
                        Console.WriteLine("Commands: info, chatlog, clear, goto, back, help, bye\nPaths: dmg, settings");
                    }
                    else
                    {
                        switch (command[1])
                        {
                            case "info":
                                Console.WriteLine("Infos about NerdyAion.");
                                break;
                            case "chatlog":
                                Console.WriteLine("chatlog [option] | options: on");
                                break;
                            case "bye":
                                Console.WriteLine("Close NerdyAion.");
                                break;
                            case "clear":
                                Console.WriteLine("Cleared the console");
                                break;
                            case "goto":
                                Console.WriteLine("Go to path x. | goto path");
                                break;
                            case "back":
                                Console.WriteLine("Go back to main path.");
                                break;
                            case "help":
                                Console.WriteLine("Shows commands and paths.");
                                break;
                            case "dmg":
                                Console.WriteLine("Path for dmg analyzing.\nCommands:\ncreate: Create a pointer (start point) for the analyzing with the given name `<pointer name>`. | create <pointer name>\nlist: Shows all pointer.\nshow: hows dmg informations from pointer `<pointer name>`. | show <pointer name>\ncopy: Cpy the dmg informations from pointer `<pointer name>`. | copy <pointer name>");
                                break;
                            case "settings":
                                Console.WriteLine("Path for handling settings.\nCommands:\nsave: Saved changes from settings\nshow: List of settings\nedit: Edit a setting | edit <setting> <value>\nundo: Reset the last changes.");
                                break;
                            default:
                                Console.WriteLine("Commands: clear, goto, back, help, bye\nPaths: settings");
                                break;
                        }
                    }
                    Console.WriteLine();
                    break;
                default:
                    Commands.ShowError("unknown command \"" + usedCommand + "\"");
                    break;
            }
        }

        private static void DmgCommands(String[] command, String usedCommand)
        {
            switch (command[0])
            {
                case "create":
                    if (command.Length < 2)
                    {
                        Commands.ShowError("unknown command \"" + usedCommand + "\"");
                    }
                    else
                    {
                        if (!Analyzer.ContainsKey(command[1]))
                        {
                            Analyzer.Add(command[1], new LogAnalyzer(Settings.GetSetting("aion") + @"\Chat.log"));
                        }
                        else
                        {
                            Commands.ShowError("pointer \"" + command[1] + "\" already exists");
                        }
                    }
                    break;
                case "reset":
                    if (command.Length < 2)
                    {
                        Commands.ShowError("unknown command \"" + usedCommand + "\"");
                    }
                    else
                    {
                        if (Analyzer.ContainsKey(command[1]))
                        {
                            Analyzer[command[1]] = new LogAnalyzer(Settings.GetSetting("aion") + @"\Chat.log");
                        }
                        else
                        {
                            Commands.ShowError("pointer \"" + command[1] + "\" don't exists");
                        }
                    }
                    break;
                case "delete":
                    if (command.Length < 2)
                    {
                        Commands.ShowError("unknown command \"" + usedCommand + "\"");
                    }
                    else
                    {
                        if (Analyzer.ContainsKey(command[1]))
                        {
                            Analyzer.Remove(command[1]);
                        }
                        else
                        {
                            Commands.ShowError("pointer \"" + command[1] + "\" don't exists");
                        }
                    }
                    break;
                case "list":
                    Console.WriteLine("=== POINTER LIST ===");
                    foreach (KeyValuePair<String, LogAnalyzer> item in Analyzer)
                    {
                        Console.WriteLine(item.Key);
                    }
                    Console.WriteLine("=====================");
                    Console.WriteLine("");
                    break;
                case "show":
                    if (command.Length < 2)
                    {
                        Commands.ShowError("unknown command \"" + usedCommand + "\"");
                    }
                    else
                    {
                        if (Analyzer.ContainsKey(command[1]))
                        {
                            Console.WriteLine("==== PLAYER LIST ====");

                            Analyzer[command[1]].AnalyzeLog();

                            String player = "";
                            foreach (KeyValuePair<String, Player> item in Analyzer[command[1]].PlayerList)
                            {
                                if (!item.Key.Equals(""))
                                {
                                    if (item.Key == "ihr" || item.Key == "Ihr")
                                    {
                                        player = Settings.GetSetting("player");
                                    }
                                    else
                                    {
                                        player = item.Key;
                                    }

                                    Console.WriteLine(player + ": " + item.Value.CalculateSkillDmg() + " (" + item.Value.GetDPS() + ")");
                                }
                            }
                            Console.WriteLine("=====================");
                            Console.WriteLine("");
                        }
                        else
                        {
                            Commands.ShowError("pointer \"" + command[1] + "\" don't exists");
                        }
                    }
                    break;
                case "copy":
                    if (command.Length < 2)
                    {
                        Commands.ShowError("unknown command \"" + usedCommand + "\"");
                    }
                    else
                    {
                        if (Analyzer.ContainsKey(command[1]))
                        {
                            Analyzer[command[1]].AnalyzeLog();

                            String dmgData = "| ";
                            String player = "";
                            foreach (KeyValuePair<String, Player> item in Analyzer[command[1]].PlayerList)
                            {
                                if (!item.Key.Equals(""))
                                {
                                    if (item.Key == "ihr" || item.Key == "Ihr")
                                    {
                                        player = Settings.GetSetting("player");
                                    }
                                    else
                                    {
                                        player = item.Key;
                                    }

                                    dmgData = player + ": " + item.Value.CalculateSkillDmg() + " (" + item.Value.GetDPS() + ")" + " | ";
                                }
                            }

                            Thread thread = new Thread(() => Clipboard.SetText(dmgData));
                            thread.SetApartmentState(ApartmentState.STA);
                            thread.Start();
                            thread.Join();
                        }
                        else
                        {
                            Commands.ShowError("pointer \"" + command[1] + "\" don't exists");
                        }
                    }
                    break;
                default:
                    Commands.BaseCommands(command, usedCommand);
                    break;
            }
        }

        private static void SettingsCommands(String[] command, String usedCommand)
        {
            switch (command[0])
            {
                case "save":
                    Console.WriteLine("Saving Settings...");
                    Settings.SaveSettings();
                    break;
                case "show":
                    Settings.LoudSettings();
                    List<Setting> settingList = Settings.GetAllSettings();

                    Console.WriteLine("=== Settzings ===");
                    for (int i = 0; i < settingList.Count; i++)
                    {
                        String saved = "";

                        if (!settingList[i].Saved)
                        {
                            saved = "[NOT SAVED] ";
                        }

                        Console.WriteLine(saved + settingList[i].Name + ": " + settingList[i].Value);
                    }
                    Console.WriteLine("");
                    break;
                case "edit":
                    if (command.Length < 3)
                    {
                        Commands.ShowError("unknown command \"" + usedCommand + "\"");
                    }
                    else
                    {
                        if (command.Length > 3)
                        {
                            for (int i = 3; i < command.Length; i++)
                            {
                                command[2] += " " + command[i];
                            }
                        }

                        if (!settings.SetSetting(command[1], command[2]))
                        {
                            Commands.ShowError("unknown setting \"" + command[1] + "\"");
                        }
                    }
                    break;
                case "undo":
                    Settings.CanselEdits();
                    break;
                default:
                    Commands.BaseCommands(command, usedCommand);
                    break;
            }
        }

        public static void ShowError(String error)
        {
            Console.WriteLine("[ERROR] " + error);
            Console.WriteLine();
        }
    }
}
