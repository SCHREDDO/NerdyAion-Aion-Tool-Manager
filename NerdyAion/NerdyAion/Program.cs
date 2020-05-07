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
// Created On: 19.02.2019
// Last Edited On: 04.05.2020
// Language: C#
//
using System;
using System.Diagnostics;
using System.IO;

namespace NerdyAion
{
    public class Program
    {
        private static ConsoleColor currentForeground = Console.ForegroundColor;
        private static ConsoleColor okColor = ConsoleColor.Green;
        private static ConsoleColor warningColor = ConsoleColor.Yellow;
        private static ConsoleColor errorColor = ConsoleColor.Red;

        static void Main(string[] args)
        {
            Boolean isError = false;
            String errorMessage = "";
            String defaultErrorMessage = "=======================================================\n" +
                "Import file/files are missing.\n" +
                "Please download the latest version of NerdyAion.\n" +
                "https://github.com/SCHREDDO/NerdyAion-Aion-Tool-Manager/releases\n" +
                "=======================================================";

            Console.WriteLine("Start...");

            //AionCFG.dll
            if (Program.CheckedStatus("AionCFG.dll", $"{AppDomain.CurrentDomain.BaseDirectory}bin{Path.DirectorySeparatorChar}AionCFG.dll"))
            {
                isError = true;
                errorMessage = defaultErrorMessage;
            }

            //Damage.dll
            if (Program.CheckedStatus("Damage.dll", $"{AppDomain.CurrentDomain.BaseDirectory}bin{Path.DirectorySeparatorChar}Damage.dll"))
            {
                isError = true;
                errorMessage = defaultErrorMessage;
            }

            Console.WriteLine("");

            //NerdyAionSystemCFGEditor.dll
            if (Program.CheckedStatus("NerdyAionSystemCFGEditor.dll", $"{AppDomain.CurrentDomain.BaseDirectory}lib{Path.DirectorySeparatorChar}NerdyAionSystemCFGEditor.dll"))
            {
                isError = true;
                errorMessage = defaultErrorMessage;
            }

            //NerdyConsoleOutput.dll
            if (Program.CheckedStatus("NerdyConsoleOutput.dll", $"{AppDomain.CurrentDomain.BaseDirectory}lib{Path.DirectorySeparatorChar}NerdyConsoleOutput.dll"))
            {
                isError = true;
                errorMessage = defaultErrorMessage;
            }

            //NerdyDamageAnalyzer.dll
            if (Program.CheckedStatus("NerdyDamageAnalyzer.dll", $"{AppDomain.CurrentDomain.BaseDirectory}lib{Path.DirectorySeparatorChar}NerdyDamageAnalyzer.dll"))
            {
                isError = true;
                errorMessage = defaultErrorMessage;
            }

            //NerdyINI.dll
            if (Program.CheckedStatus("NerdyINI.dll", $"{AppDomain.CurrentDomain.BaseDirectory}lib{Path.DirectorySeparatorChar}NerdyINI.dll"))
            {
                isError = true;
                errorMessage = defaultErrorMessage;
            }

            //NerdyLogReader.dll
            if (Program.CheckedStatus("NerdyLogReader.dll", $"{AppDomain.CurrentDomain.BaseDirectory}lib{Path.DirectorySeparatorChar}NerdyLogReader.dll"))
            {
                isError = true;
                errorMessage = defaultErrorMessage;
            }

            Console.WriteLine("");

            //NerdyConsole.exe
            if (Program.CheckedStatus("NerdyConsole.exe", $"{AppDomain.CurrentDomain.BaseDirectory}NerdyConsole.exe"))
            {
                isError = true;
                errorMessage = defaultErrorMessage;
            }

            //nerdy.ini
            if (Program.CheckedStatus("nerdy.ini", $"{AppDomain.CurrentDomain.BaseDirectory}nerdy.ini"))
            {
                isError = true;
                Console.WriteLine("generate default nerdy.ini...");
                using (StreamWriter sw = File.CreateText($"{AppDomain.CurrentDomain.BaseDirectory}nerdy.ini"))
                {
                    sw.WriteLine(@"[info]");
                    sw.WriteLine(@"name=NerdyAion");
                    sw.WriteLine(@"version=2.1.0");
                    sw.WriteLine(@"repository=https://github.com/SCHREDDO/NerdyAion-Aion-Tool-Manager");
                    sw.WriteLine(@"author=SCHREDDO");
                }
                Console.WriteLine("");
            }

            //base.conf
            if (Program.CheckedStatus("base.conf", $"{AppDomain.CurrentDomain.BaseDirectory}etc{Path.DirectorySeparatorChar}base.conf"))
            {
                isError = true;
                Console.WriteLine("generate default base.conf...");
                using (StreamWriter sw = File.CreateText($"{AppDomain.CurrentDomain.BaseDirectory}etc{Path.DirectorySeparatorChar}base.conf"))
                {
                    sw.WriteLine(@"language=EN");
                    sw.WriteLine(@"aion=C:\Program Files\Gameforge\AION Free-To-Play");
                    sw.WriteLine(@"check_chatlog_active=1");
                    sw.WriteLine(@"player=me");
                    sw.WriteLine(@"dmg_template=<player>: <dmg>(<dps>)");
                    sw.WriteLine(@"show_max=24");
                    sw.WriteLine(@"sort_by=dmg");
                    sw.WriteLine(@"check_version=1");
                    sw.WriteLine(@"new_version_browser=0");
                }
                Console.WriteLine("");
            }

            Console.WriteLine("");

            Console.WriteLine("check if Aion path is correct...");
            using (StreamReader sr = new StreamReader($"{AppDomain.CurrentDomain.BaseDirectory}etc{Path.DirectorySeparatorChar}base.conf"))
            {
                String line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    string[] temp = line.Split(new Char[] { '=' }, 2);
                    if (temp[0] == "aion")
                    {
                        if (!File.Exists($"{temp[1]}{Path.DirectorySeparatorChar}system.cfg"))
                        {
                            isError = true;
                            Console.WriteLine("");
                            Console.ForegroundColor = Program.warningColor;
                            Console.WriteLine("=======================================================");
                            Console.WriteLine("The given aion path is not correct.");
                            Console.WriteLine("Pleas set the correct one with 'settings edit -s aion <your_aion_path>'.");
                            Console.WriteLine("======================================================="); ;
                            Console.ForegroundColor = Program.currentForeground;
                        }

                        if (!File.Exists($"{temp[1]}{Path.DirectorySeparatorChar}Chat.log"))
                        {
                            isError = true;
                            Console.WriteLine("");
                            Console.ForegroundColor = Program.warningColor;
                            Console.WriteLine("=======================================================");
                            Console.WriteLine("Chat log seems do not be active.");
                            Console.WriteLine("Pleas use 'chatlog on' before you start Aion.");
                            Console.WriteLine("======================================================="); ;
                            Console.ForegroundColor = Program.currentForeground;
                        }

                        break;
                    }
                }

                sr.Close();
            }

            Console.WriteLine("");

            Console.WriteLine("check if Aion is running...");
            if (Process.GetProcessesByName("notepad").Length > 0)
            {
                isError = true;
                errorMessage = "=======================================================\n" +
                "Aion is running.\n" +
                "Pleas close Aion and start NerdyAion again.\n" +
                "=======================================================";
            }


            Console.WriteLine("");

            if (isError)
            {
                Console.ForegroundColor = Program.errorColor;
                Console.WriteLine(errorMessage);
                Console.ForegroundColor = Program.currentForeground;
                Console.WriteLine("PRESS ENTER...");
                Console.ReadLine();
            }

            if (errorMessage.Equals(""))
            {
                Console.WriteLine("Starting NerdyConsole.exe...");
                Process.Start("NerdyConsole.exe");
            }
        }

        public static Boolean CheckedStatus(String name, String path)
        {

            Console.Write($"{name}: ");
            if (!File.Exists(path))
            {
                Console.ForegroundColor = Program.errorColor;
                Console.WriteLine("ERROR");
                Console.ForegroundColor = Program.currentForeground;

                return true;
            }
            else
            {
                Console.ForegroundColor = Program.okColor;
                Console.WriteLine("OK");
                Console.ForegroundColor = Program.currentForeground;

                return false;
            }
        }
    }
}
