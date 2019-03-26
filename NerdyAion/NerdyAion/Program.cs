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
// Last Edited On: 26.03.2019
// Language: C#
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace NerdyAion
{
    public class Program
    {
        static void Main(string[] args)
        {
            Commands.Analyzer = new Dictionary<string, LogAnalyzer>();
            Commands.Settings = new SettingsController(System.AppDomain.CurrentDomain.BaseDirectory + @"\settings.txt", false);

            if (!Commands.Settings.LoudSettings())
            {
                Commands.Settings.CreateSettingsFile();
                Commands.Settings.AddSetting("version", "1.0.0", true, null);
                Commands.Settings.AddSetting("aion", @"C:\Program Files\Gameforge\AION Free-To-Play");
                Commands.Settings.AddSetting("player", "ich");
                Commands.Settings.AddSetting("chatlog_active", "0");
                Commands.Settings.AddSetting("languarge", "DE");
                Commands.Settings.SaveSettings();
            }
            else
            {
                if (Commands.Settings.GetSetting("version") != "1.0.0")
                {
                    SettingsController temp = new SettingsController(System.AppDomain.CurrentDomain.BaseDirectory + @"\settings.txt", false);
                    temp.AddSetting("version", "1.0.0", true, null);
                    temp.AddSetting("aion", @"C:\Program Files\Gameforge\AION Free-To-Play");
                    temp.AddSetting("check_chatlog_active", "0");
                    temp.AddSetting("languarge", "DE");

                    foreach (Setting item in Commands.Settings.GetAllSettings())
                    {
                        if (item.Name != "version")
                        {
                            temp.AddSetting(item.Name, item.Value);
                        }
                    }

                    temp.SaveSettings();
                    Commands.Settings = temp;
                }
            }
        
            if (Commands.Settings.GetSetting("check_chatlog_active") == "1")
            {
                SystemCFGEditor.SetChatLogActive(Commands.Settings.GetSetting("aion") + @"\system.cfg");
            }

            Commands.ActivCommantLevel = CommantLevel.BaseLevel;

            Console.WriteLine("===============================");
            Console.WriteLine("========== NerdyAion ==========");
            Console.WriteLine("===============================");

            String command = "";
            Console.Write(Commands.ActivCommantLevel);
            while ((command = Console.ReadLine()) != "bye")
            {
                Commands.Execut(command);
                Console.Write(Commands.ActivCommantLevel);
            }
        }
    }
}
