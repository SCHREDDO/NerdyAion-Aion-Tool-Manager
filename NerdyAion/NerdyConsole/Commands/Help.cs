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

namespace NerdyConsole.Commands
{
    public class Help : BaseCommand
    {
        public Dictionary<String, Object> CommadsList { get; set; }

        public override int Initialize(Dictionary<string, string> baseSettings, Dictionary<string, string> commandSettings)
        {
            return 0;
        }

        public override int Execute(string command, object[] args, Dictionary<string, string> baseSettings, Dictionary<string, string> commandSettings)
        {
            int code = 0;

            if (args.Length == 0)
            {
                foreach (KeyValuePair<String, Object> commad in CommadsList)
                {
                    var method = commad.Value.GetType().GetMethod("CHelp", new Type[] { typeof(Boolean) });
                    ConsoleOut(commad.Key + ": " + (String)method.Invoke(commad.Value, new object[] { true }));
                }

                ConsoleOut("");
            }
            else if (args.Length == 1)
            {
                if (CommadsList.ContainsKey(args[0].ToString()))
                {
                    var method = ((object)CommadsList[args[0].ToString()]).GetType().GetMethod("CHelp", new Type[0]);

                    ConsoleOut(method.Invoke(CommadsList[args[0].ToString()], new object[0]).ToString());
                    ConsoleOut("");
                }
                else
                {
                    code = 2;
                }
            }
            else
            {
                code = 1;
            }

            return code;
        }

        public override Dictionary<string, string> CInfo()
        {
            Dictionary<String, String> info = new Dictionary<String, String>();
            info.Add("name", "Help");
            info.Add("version", "1.0.0");
            info.Add("command", "help");
            info.Add("i1", "Unknown argument or arguments. Try 'help help' for help.");
            info.Add("i2", "Unknown command. Try 'help' to get a list of commads.");

            return info;
        }

        public override string CHelp()
        {
            return "Provides information about commands. If used without parameters a lists of all commands are displayed without a briefly description.\n\n"
                     + "Syntax:\n"
                     + "```console\n"
                     + "help [command]\n"
                     + "```\n\n"
                     + "Arguments:\n"
                     + "-`command`: the command you wish to receive more information about\n\n"
                     + "Example:\n"
                     + "```console\n"
                     + "help\n"
                     + "help dmg\n"
                     + "```";
        }

        public override string CHelp(bool smallInfo)
        {
            String info = "shows commands and information";

            if (!smallInfo)
            {
                info = CHelp();
            }

            return info;
        }
    }
}
