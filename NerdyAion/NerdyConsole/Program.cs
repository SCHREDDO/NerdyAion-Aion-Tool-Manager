// Copyright (C) 2019-2020 Sebastian Lühnen
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
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with NerdyAion. If not, see <http://www.gnu.org/licenses/>.
//
//
// Created By: Sebastian Lühnen
// Created On: 06.04.2019
// Last Edited On: 21.04.2020
// Language: C#
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using NerdyConsole.Commands;
using NerdyConsole.Models;

namespace NerdyConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            SettingsController settingsController = new SettingsController(@"etc\" + "base.conf", true);
            Dictionary<String, String> settings = settingsController.GetSettings();

            String path = AppDomain.CurrentDomain.BaseDirectory + @"lib\" + "NerdyConsoleOutput.dll";

            Type consoleOutput;
            Type nerdyINI;
            Object nerdyINIObj;
            Assembly assembly = Assembly.LoadFrom(path);
            consoleOutput = assembly.GetType("NerdyConsoleOutput.NerdyConsoleOutput");

            path = AppDomain.CurrentDomain.BaseDirectory + @"lib\" + "NerdyINI.dll";
            assembly = Assembly.LoadFrom(path);
            nerdyINI = assembly.GetType("NerdyINI.NerdyINI");
            nerdyINIObj = Activator.CreateInstance(nerdyINI, new object[] { AppDomain.CurrentDomain.BaseDirectory + "nerdy.ini" });

            consoleOutput.GetMethod("Output").Invoke(null, new object[] { "===============================" });
            consoleOutput.GetMethod("Output").Invoke(null, new object[] { "========== NerdyAion ==========" });
            consoleOutput.GetMethod("Output").Invoke(null, new object[] { "===============================" });
            consoleOutput.GetMethod("Output").Invoke(null, new object[] { "" });

            if (settings["check_chatlog_active"] == "1")
            {
                path = AppDomain.CurrentDomain.BaseDirectory + @"lib\" + "NerdyAionSystemCFGEditor.dll";

                Type nerdyAionSystemCFGEditor;
                assembly = Assembly.LoadFrom(path);
                nerdyAionSystemCFGEditor = assembly.GetType("NerdyAionSystemCFGEditor.NerdyAionSystemCFGEditor");
                nerdyAionSystemCFGEditor.GetMethod("SetChatLogActive").Invoke(null, new object[] { settings["aion"] + @"\system.cfg" });
            }

            int code = 0;
            String input = "";
            String command = "";
            String[] commandArgs = new String[0];
            List<String> commandParts = new List<String>();
            String helpCommand = new Help().CInfo()["command"];
            String settingsCommand = new Settings().CInfo()["command"];
            Object obj = null;

            Dictionary<String, String> info = new Dictionary<String, String>();

            Dictionary<String, Object> commands = new Dictionary<String, Object>();
            commands.Add(new Info().CInfo()["command"], new Info());
            commands.Add(new Help().CInfo()["command"], new Help());
            commands.Add(new Settings().CInfo()["command"], new Settings());
            commands.Add(new Clear().CInfo()["command"], new Clear());
            commands.Add(new Bye().CInfo()["command"], new Bye());

            DirectoryInfo directory = new DirectoryInfo(@"bin\");
            FileInfo[] Files = directory.GetFiles("*.dll");
            
            foreach (FileInfo file in Files)
            {
                Type temp = null;
                
                assembly = Assembly.LoadFrom(file.FullName);
                temp = assembly.GetType(file.Name.Replace(".dll", "") + "." + file.Name.Replace(".dll", ""));
                obj = Activator.CreateInstance(temp, new object[0]);
                info = (Dictionary<String, String>)temp.GetMethod("CInfo").Invoke(obj, new object[0]);

                commands.Add(info["command"], obj);
            }
            
            ((Help)commands[new Help().CInfo()["command"]]).CommadsList = commands;

            foreach (KeyValuePair<String, Object> temp in commands)
            {
                var method = (temp.Value).GetType().GetMethod("Initialize");
                code = (int)method.Invoke(temp.Value, new object[] { settings, settings });
            }

            code = 0;
            while (code != -1)
            {
                input = "";
                command = "";
                commandParts = new List<String>();
                commandArgs = new String[0];

                Console.Write(">");
                input = Console.ReadLine();
                commandParts = Regex.Matches(input, @"[\""].+?[\""]|[^\s]+")
                                .Cast<Match>()
                                .Select(m => m.Value.Trim('"'))
                                .ToList();

                if (commandParts.Count > 0)
                {
                    command = commandParts[0];
                    if (commands.ContainsKey(command))
                    {
                        if (commandParts.Count > 1)
                        {
                            commandArgs = new String[commandParts.Count - 1];
                            Array.Copy(commandParts.ToArray(), 1, commandArgs, 0, commandArgs.Length);
                        }

                        if (command.Equals(helpCommand))
                        {
                            var method = ((object)commands[command]).GetType().GetMethod("Execute");
                            code = (int)method.Invoke(commands[command], new object[] { command, commandArgs, settings, settings });
                        }
                        else
                        {
                            var method = ((object)commands[command]).GetType().GetMethod("Execute");
                            code = (int)method.Invoke(commands[command], new object[] { command, commandArgs, settings, settings });
                        }

                        if (command.Equals(settingsCommand))
                        {
                            settings = ((Settings)commands[new Settings().CInfo()["command"]]).SettingsController.GetSettings();
                        }
                    }
                    else
                    {
                        consoleOutput.GetMethod("Output").Invoke(obj, new object[] { "Unknown command. Try 'help' for help." });
                        consoleOutput.GetMethod("Output").Invoke(obj, new object[] { "" });
                    }

                    if (code > 0)
                    {
                        var method = ((object)commands[command]).GetType().GetMethod("CInfo");
                        info = (Dictionary<String, String>)method.Invoke(commands[command], new object[0]);
                        consoleOutput.GetMethod("Output").Invoke(obj, new object[] { info["i" + code] });
                        consoleOutput.GetMethod("Output").Invoke(obj, new object[] { "" });
                    }
                }
            }

            consoleOutput.GetMethod("Output").Invoke(obj, new object[] { "Press Enter..." });
            Console.ReadLine();
        }
    }
}
