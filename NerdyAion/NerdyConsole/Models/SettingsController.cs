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
// Last Edited On: 21.08.2020
// Language: C#
//
using System;
using System.Collections.Generic;
using System.IO;

namespace NerdyConsole.Models
{
    public class SettingsController
    {
        private String settinFilePath;
        private Dictionary<String, Setting> settings;

        private String SettinFilePath
        {
            get { return settinFilePath; }
            set { settinFilePath = value; }
        }
        private Dictionary<String, Setting> Settings
        {
            get { return settings; }
            set { settings = value; }
        }

        public SettingsController(String settinFilePath, Boolean loudByStart = true)
        {
            SettinFilePath = settinFilePath;
            Settings = new Dictionary<string, Setting>();

            if (loudByStart)
            {
                LoudSettings();
            }
        }

        public Boolean CreateSettingsFile()
        {
            try
            {
                FileStream fs = File.Create(SettinFilePath);
                fs.Close();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public String GetSetting(String setting)
        {
            if (!Settings.ContainsKey(setting))
            {
                return null;
            }

            return Settings[setting].Value;
        }

        public Dictionary<String, String> GetSettings()
        {
            Dictionary<String, String> settings = new Dictionary<String, String>();

            foreach (var setting in Settings)
            {
                settings.Add(setting.Value.Name, setting.Value.Value);
            }

            return settings;
        }

        public List<Setting> GetAllSettings()
        {
            List<Setting> settingList = new List<Setting>();

            foreach (var setting in Settings)
            {
                settingList.Add(setting.Value);
            }

            return settingList;
        }

        public Boolean SetSetting(String setting, String value)
        {
            if (!Settings.ContainsKey(setting))
            {
                return false;
            }

            Settings[setting].Value = value;
            Settings[setting].Saved = false;

            return true;
        }

        public Boolean AddSetting(String setting, String value)
        {
            return AddSetting(new Setting(setting, value));
        }

        private bool AddSetting(Setting setting)
        {
            if (Settings.ContainsKey(setting.Name))
            {
                return false;
            }

            Settings.Add(setting.Name, setting);

            return true;
        }

        public Boolean SaveSettings()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(SettinFilePath))
                {
                    foreach (var setting in Settings)
                    {
                        String line = "";

                        line += setting.Value.Name;
                        line += "=" + setting.Value.Value;

                        sw.WriteLine(line);

                        setting.Value.Saved = true;
                    }

                    sw.Close();
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public Boolean CanselEdits()
        {
            settings.Clear();
            return LoudSettings();
        }

        public Boolean LoudSettings()
        {
            try
            {
                string line = "";
                using (StreamReader sr = new StreamReader(SettinFilePath))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] temp = line.Split(new Char[] { '=' }, 2);
                        AddSetting(temp[0], temp[1]);
                    }

                    sr.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
        }
    }
}
