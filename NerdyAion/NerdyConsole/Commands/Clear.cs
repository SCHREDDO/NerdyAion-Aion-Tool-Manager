﻿// Copyright (C) 2019-2020 Sebastian Lühnen
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
    public class Clear : BaseCommand
    {
        public override int Initialize(Dictionary<string, string> baseSettings, Dictionary<string, string> commandSettings)
        {
            return 0;
        }

        public override int Execute(string command, object[] args, Dictionary<string, string> baseSettings, Dictionary<string, string> commandSettings)
        {
            int code = 0;

            if (args.Length == 0)
            {
                Console.Clear();
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
            info.Add("name", "Clear");
            info.Add("version", "1.0.0");
            info.Add("command", "clear");
            info.Add("i1", "Unknown argument or arguments. Try 'help clear' for help.");

            return info;
        }

        public override string CHelp()
        {
            return "Deletes the display information of the current console window.\n\n"
                    + "Syntax:\n"
                    + "```console\n"
                    + "clear\n"
                    + "```\n\n"
                    + "Example:\n"
                    + "```console\n"
                    + "clear\n"
                    + "```";
        }

        public override string CHelp(bool smallInfo)
        {
            String info = "clears the console";

            if (!smallInfo)
            {
                info = CHelp();
            }

            return info;
        }
    }
}
