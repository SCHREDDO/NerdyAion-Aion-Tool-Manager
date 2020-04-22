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
using System.Text;
using System.Threading.Tasks;

namespace NerdyAionSystemCFGEditor
{
    public class NerdyAionSystemCFGEditor
    {
        private List<String> systemcfgLines;

        public List<String> SystemcfgLines
        {
            get { return systemcfgLines; }
            set { systemcfgLines = value; }
        }

        public NerdyAionSystemCFGEditor()
        {
            SystemcfgLines = new List<String>();
        }

        public bool ReadSystemCFG(String path)
        {
            return false;
        }

        public bool WriteSystemCFG(String path, List<String> variables)
        {
            return false;
        }

        public bool IsVariableSet(String variable)
        {
            return false;
        }

        public bool HasValue(String variable, String value)
        {
            return false;
        }

        public bool AddVariables(String variable, String value)
        {
            return false;
        }

        public bool EditVariables(String variable, String value)
        {
            return false;
        }

        public bool DeleteVariables(String variable)
        {
            return false;
        }

        private int GetIndexOfVariable(String variable)
        {
            return -1;
        }

        public static void SetChatLogActive(String path)
        {
            String systemcfg = path;
            List<String> systemcfgLines = new List<String>();
            String tempString = "";
            bool endOfLine = false;
            bool chatlogSet = false;
            bool chatlogActiv = false;
            byte[] tempBytes;
            char tempChar;

            try
            {
                FileStream systemcfgFile = new FileStream(systemcfg, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(systemcfgFile, Encoding.ASCII);

                systemcfgLines.Add(Encoding.ASCII.GetString(br.ReadBytes(33)));
                systemcfgLines[0] = systemcfgLines[0].Replace("\r", "");
                systemcfgLines[0] = systemcfgLines[0].Replace("\n", "");
                systemcfgLines.Add(Encoding.ASCII.GetString(br.ReadBytes(96)));
                systemcfgLines[1] = systemcfgLines[1].Replace("\r", "");
                systemcfgLines[1] = systemcfgLines[1].Replace("\n", "");

                while (true)
                {
                    try
                    {
                        tempChar = (char)br.ReadByte();
                        if (tempChar != '\r' && tempChar != '\n')
                        {
                            if (endOfLine)
                            {
                                systemcfgLines.Add(tempString);
                                tempString = "";
                                endOfLine = false;
                            }
                            tempBytes = new byte[] { (byte)(char)~tempChar };
                            tempString += Encoding.ASCII.GetString(tempBytes);
                        }
                        else
                        {
                            endOfLine = true;
                        }
                    }
                    catch
                    {
                        break;
                    }
                }

                if (tempString != "")
                {
                    systemcfgLines.Add(tempString);
                    tempString = "";
                    endOfLine = false;
                }

                br.Close();
                systemcfgFile.Close();
            }
            catch
            {

            }

            for (int i = 0; i < systemcfgLines.Count; i++)
            {
                if (systemcfgLines[i] == "g_chatlog = \"1\"" || systemcfgLines[i] == "g_chatlog = \"0\"")
                {
                    chatlogSet = true;
                    if (systemcfgLines[i] == "g_chatlog = \"1\"")
                    {
                        chatlogActiv = true;
                    }
                }
            }

            if (!chatlogSet)
            {
                systemcfgLines.Add("g_chatlog = \"1\"");
            }
            else if (!chatlogActiv)
            {
                systemcfgLines.Remove("g_chatlog = \"0\"");
                systemcfgLines.Add("g_chatlog = \"1\"");
            }

            try
            {
                FileStream systemcfgFile = new FileStream(systemcfg, FileMode.Open, FileAccess.Write);
                BinaryWriter bw = new BinaryWriter(systemcfgFile, Encoding.ASCII);

                systemcfgFile.SetLength(0);

                tempBytes = Encoding.ASCII.GetBytes(systemcfgLines[0]);
                bw.Write(tempBytes);

                tempBytes = Encoding.ASCII.GetBytes("\r\n");
                bw.Write(tempBytes);

                tempBytes = Encoding.ASCII.GetBytes(systemcfgLines[1]);
                bw.Write(tempBytes);

                tempBytes = Encoding.ASCII.GetBytes("\r\n");
                bw.Write(tempBytes);

                for (int i = 2; i < systemcfgLines.Count; i++)
                {
                    tempBytes = Encoding.ASCII.GetBytes(systemcfgLines[i]);

                    for (int y = 0; y < tempBytes.Length; y++)
                    {
                        tempBytes[y] = (byte)~tempBytes[y];
                    }
                    bw.Write(tempBytes);

                    tempBytes = Encoding.ASCII.GetBytes("\r\n");
                    bw.Write(tempBytes);
                }

                bw.Close();
                systemcfgFile.Close();
            }
            catch
            {

            }
        }

        public static void SetChatLogInactive(String path)
        {
            String systemcfg = path;
            List<String> systemcfgLines = new List<String>();
            String tempString = "";
            bool endOfLine = false;
            bool chatlogSet = false;
            bool chatlogActiv = false;
            byte[] tempBytes;
            char tempChar;

            try
            {
                FileStream systemcfgFile = new FileStream(systemcfg, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(systemcfgFile, Encoding.ASCII);

                systemcfgLines.Add(Encoding.ASCII.GetString(br.ReadBytes(33)));
                systemcfgLines[0] = systemcfgLines[0].Replace("\r", "");
                systemcfgLines[0] = systemcfgLines[0].Replace("\n", "");
                systemcfgLines.Add(Encoding.ASCII.GetString(br.ReadBytes(96)));
                systemcfgLines[1] = systemcfgLines[1].Replace("\r", "");
                systemcfgLines[1] = systemcfgLines[1].Replace("\n", "");

                while (true)
                {
                    try
                    {
                        tempChar = (char)br.ReadByte();
                        if (tempChar != '\r' && tempChar != '\n')
                        {
                            if (endOfLine)
                            {
                                systemcfgLines.Add(tempString);
                                tempString = "";
                                endOfLine = false;
                            }
                            tempBytes = new byte[] { (byte)(char)~tempChar };
                            tempString += Encoding.ASCII.GetString(tempBytes);
                        }
                        else
                        {
                            endOfLine = true;
                        }
                    }
                    catch
                    {
                        break;
                    }
                }

                if (tempString != "")
                {
                    systemcfgLines.Add(tempString);
                    tempString = "";
                    endOfLine = false;
                }

                br.Close();
                systemcfgFile.Close();
            }
            catch
            {

            }

            systemcfgLines.Remove("g_chatlog = \"0\"");
            systemcfgLines.Remove("g_chatlog = \"1\"");

            try
            {
                FileStream systemcfgFile = new FileStream(systemcfg, FileMode.Open, FileAccess.Write);
                BinaryWriter bw = new BinaryWriter(systemcfgFile, Encoding.ASCII);

                systemcfgFile.SetLength(0);

                tempBytes = Encoding.ASCII.GetBytes(systemcfgLines[0]);
                bw.Write(tempBytes);

                tempBytes = Encoding.ASCII.GetBytes("\r\n");
                bw.Write(tempBytes);

                tempBytes = Encoding.ASCII.GetBytes(systemcfgLines[1]);
                bw.Write(tempBytes);

                tempBytes = Encoding.ASCII.GetBytes("\r\n");
                bw.Write(tempBytes);

                for (int i = 2; i < systemcfgLines.Count; i++)
                {
                    tempBytes = Encoding.ASCII.GetBytes(systemcfgLines[i]);

                    for (int y = 0; y < tempBytes.Length; y++)
                    {
                        tempBytes[y] = (byte)~tempBytes[y];
                    }
                    bw.Write(tempBytes);

                    tempBytes = Encoding.ASCII.GetBytes("\r\n");
                    bw.Write(tempBytes);
                }

                bw.Close();
                systemcfgFile.Close();
            }
            catch
            {

            }
        }
    }
}
