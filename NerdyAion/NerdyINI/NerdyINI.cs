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
// Last Edited On: 04.05.2020
// Language: C#
//
using System;
using System.Collections.Generic;
using System.IO;

namespace NerdyINI
{
    public class NerdyINI
    {
        private String iNIPath;
        private Dictionary<String, Dictionary<String, String>> variables;

        private String INIPath
        {
            get { return iNIPath; }
            set { iNIPath = value; }
        }
        private Dictionary<String, Dictionary<String, String>> Variables
        {
            get { return variables; }
            set { variables = value; }
        }

        public NerdyINI(String iniPath)
        {
            INIPath = iniPath;
            Variables = new Dictionary<string, Dictionary<string, string>>();

            LoudINI();
        }

        public void LoudINI()
        {
            ReadINI();
        }

        public void SaveINI()
        {
            WriteINI();
        }

        private void ReadINI()
        {
            try
            {
                StreamReader iniReader = new StreamReader(INIPath);
                String temp = "";
                String activeGroup = "non";
                while ((temp = iniReader.ReadLine()) != null)
                {
                    if (temp.StartsWith("[") && temp.EndsWith("]"))
                    {
                        activeGroup = temp.Substring(1, temp.Length - 2);
                        if (!Variables.ContainsKey(activeGroup))
                        {
                            Variables.Add(activeGroup, new Dictionary<string, string>());
                        }
                    }
                    else if (temp.Contains("="))
                    {
                        String[] variable = temp.Split(new Char[] { '=' }, 2);

                        if (!Variables.ContainsKey(activeGroup))
                        {
                            Variables.Add(activeGroup, new Dictionary<string, string>());
                        }

                        if (!Variables[activeGroup].ContainsKey(variable[0]))
                        {
                            Variables[activeGroup].Add(variable[0], variable[1]);
                        }
                    }
                }

                iniReader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void WriteINI()
        {
            try
            {
                StreamWriter iniWrite = new StreamWriter(INIPath);

                foreach (KeyValuePair<String, Dictionary<String, String>> group in Variables)
                {
                    if (group.Key != "non")
                    {
                        iniWrite.WriteLine("[" + group.Key + "]");
                    }

                    foreach (KeyValuePair<String, String> variable in group.Value)
                    {
                        iniWrite.WriteLine(variable.Key + "=" + variable.Value);
                    }

                    iniWrite.WriteLine("");
                }

                iniWrite.Flush();
                iniWrite.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void AddGroup(String group)
        {
            if (!Variables.ContainsKey(group))
            {
                Variables.Add(group, new Dictionary<string, string>());
            }
        }

        public void AddVariable(String variable, String value, String group)
        {
            if (Variables.ContainsKey(group))
            {
                Variables[group].Add(variable, value);
            }
            else
            {
                AddGroup(group);
                AddVariable(variable, value, group);
            }
        }

        public Dictionary<String, String> GetGroup(String group)
        {
            if (Variables.ContainsKey(group))
            {
                return Variables[group];
            }

            return new Dictionary<string, string>();
        }

        public String GetVariable(String variable)
        {
            String temp = null;

            foreach (KeyValuePair<String, Dictionary<String, String>> group in Variables)
            {
                temp = GetVariable(variable, group.Key);

                if (temp != null)
                {
                    break;
                }
            }

            return temp;
        }

        public String GetVariable(String variable, String group)
        {
            if (Variables.ContainsKey(group))
            {
                if (Variables[group].ContainsKey(variable))
                {
                    return Variables[group][variable];
                }
            }

            return null;
        }
    }
}
