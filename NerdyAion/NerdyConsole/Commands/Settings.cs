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
using NerdyConsole.Models;

namespace NerdyConsole.Commands
{
    public class Settings : BaseCommand
    {
        public SettingsController SettingsController { get; set; }

        public override int Initialize(Dictionary<string, string> baseSettings, Dictionary<string, string> commandSettings)
        {
            SettingsController = new SettingsController(@"etc\" + "base.conf", true);

            return 0;
        }

        public override int Execute(string command, object[] args, Dictionary<string, string> baseSettings, Dictionary<string, string> commandSettings)
        {
            int code = 0;

            if (args.Length == 0)
            {
                code = 1;
            }
            else if (args.Length == 1)
            {
                switch (args[0])
                {
                    case "show":
                        List<Setting> settingList = SettingsController.GetAllSettings();
                        ConsoleOut("========== Settzings ==========");
                        for (int i = 0; i < settingList.Count; i++)
                        {
                            String saved = "";

                            if (!settingList[i].Saved)
                            {
                                saved = "[NOT SAVED] ";
                            }

                            ConsoleOut($"{saved}{settingList[i].Name}: {settingList[i].Value}");
                        }
                        ConsoleOut("");
                        break;
                    case "save":
                        SettingsController.SaveSettings();
                        break;
                    case "undo":
                        SettingsController.LoudSettings();
                        break;
                    case "reset":
                        SettingsController.SetSetting("language", "EN");
                        SettingsController.SetSetting("aion", @"C:\Program Files\Gameforge\AION Free-To-Play");
                        SettingsController.SetSetting("check_chatlog_active", "0");
                        SettingsController.SetSetting("player", "me");
                        SettingsController.SetSetting("show_max", "24");
                        SettingsController.SetSetting("sort_by", "dmg");
                        SettingsController.SetSetting("dmg_template", "<player>: <dmg>(<dps>)");
                        SettingsController.SetSetting("check_version", "1");
                        SettingsController.SetSetting("new_version_browser", "0");
                        break;
                    default:
                        code = 1;
                        break;
                }
            }
            else if (args.Length >= 3 && args.Length <= 4)
            {
                if ((String)args[0] == "edit")
                {
                    if (args.Length == 3)
                    {
                        if (!SettingsController.SetSetting((String)args[1], (String)args[2]))
                        {
                            code = 2;
                        }
                    }
                    else
                    {
                        if (!SettingsController.SetSetting((String)args[2], (String)args[3]))
                        {
                            code = 2;
                        }
                    }

                    if ((String)args[1] == "-s")
                    {
                        SettingsController.SaveSettings();
                    }
                }
                else
                {
                    code = 1;
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
            info.Add("name", "Settings");
            info.Add("version", "1.0.0");
            info.Add("command", "settings");
            info.Add("i1", "Unknown argument or arguments. Try 'help settings' for help.");
            info.Add("i2", "Unknown setting.");

            return info;
        }

        public override string CHelp()
        {
            return "For handling application settings.\n\n"
                    + "Syntax:\n"
                    + "``console\n"
                    + "settings <show | edit | save | undo | reset> [-s] [setting] [value]\n"
                    + "```\n"
                    + "```console\n"
                    + "settings show\n"
                    + "settings edit [-s] <setting> <value>\n"
                    + "settings save\n"
                    + "settings undo\n"
                    + "settings reset\n"
                    + "```\n\n"
                    + "Arguments:\n"
                    + "-`show`: shows all settings and values\n"
                    + "-`edit`: edit the `<setting>` with the new value `<value>`\n"
                    + "-`save`: saved changes from settings\n"
                    + "-`undo`: reset the last changes from settings, only if not saved\n"
                    + "-`reset`: reset the last changes from settings, only if not saved\n"
                    + "-`-s`: for saving instantly\n"
                    + "-`setting`: which setting is to be changed\n"
                    + "-`value`: what value the setting should be set to\n\n"
                    + "Example:\n"
                    + "```console\n"
                    + "settings show\n"
                    + "settings edit language EN\n"
                    + "settings edit - s language DE\n"
                    + "settings save\n"
                    + "settings undo\n"
                    + "settings reset\n"
                    + "```";
        }

        public override string CHelp(bool smallInfo)
        {
            String info = "for handling application settings";

            if (!smallInfo)
            {
                info = CHelp();
            }

            return info;
        }
    }
}
