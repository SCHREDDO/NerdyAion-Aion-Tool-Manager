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
using System.Threading;
using System.Windows.Forms;

namespace Damage
{
    public class Damage : BaseCommand
    {
        public Dictionary<string, string> Analyzer { get; set; }

        public override int Initialize(Dictionary<string, string> baseSettings, Dictionary<string, string> commandSettings)
        {
            Analyzer = new Dictionary<string, string>();

            return 0;
        }

        public override int Execute(string command, object[] args, Dictionary<string, string> baseSettings, Dictionary<string, string> commandSettings)
        {
            int code = 0;
            if (args.Length == 1)
            {
                if ((String)args[0] == "list")
                {
                    ConsoleOut("======== POINTER LIST ========");
                    foreach (KeyValuePair<string, string> item in Analyzer)
                    {
                        ConsoleOut(item.Key);
                    }
                    ConsoleOut("");
                }
                else
                {
                    code = 1;
                }
            }
            else if (args.Length >= 2)
            {
                switch (args[0])
                {
                    case "add":
                        if (args.Length == 2)
                        {
                            if (!Analyzer.ContainsKey((String)args[1]))
                            {
                                Obj($"nda{args[1].ToString()}", "NerdyDamageAnalyzer.NerdyDamageAnalyzer", new object[] { $"{baseSettings["aion"]}{Path.DirectorySeparatorChar}Chat.log", baseSettings["language"], baseSettings["player"]}, this.CPatch("lib", "NerdyDamageanAlyzer.dll"));
                                Analyzer.Add(args[1].ToString(), $"nda{args[1].ToString()}");
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
                        break;
                    case "clear":
                        if (args.Length == 2)
                        {
                            if (!Analyzer.ContainsKey((String)args[1]))
                            {
                                ExMethod(Analyzer[(String)args[1]], "Clear", new object[] { }, new Type[] { });
                            }
                            else
                            {
                                code = 3;
                            }
                        }
                        else
                        {
                            code = 1;
                        }
                        break;
                    case "remove":
                        if (args.Length == 2)
                        {
                            if (!Analyzer.ContainsKey((String)args[1]))
                            {
                                RemoveObj(Analyzer[(String)args[1]]);
                                Analyzer.Remove((String)args[1]);
                            }
                            else
                            {
                                code = 3;
                            }
                        }
                        else
                        {
                            code = 1;
                        }
                        break;
                    case "show":
                        if (args.Length == 2)
                        {
                            if (Analyzer.ContainsKey((String)args[1]))
                            {
                                ExMethod(Analyzer[(String)args[1]], "AnalyzeLog", new object[] { }, new Type[] { });

                                ConsoleOut("==== PLAYER LIST ====");

                                List<String> temp = new List<String>();
                                temp = (List<String>)ExMethod(Analyzer[(String)args[1]], "Show", new object[] { baseSettings["dmg_template"], baseSettings["sort_by"], Convert.ToInt32(baseSettings["show_max"]) }, new Type[] { typeof(String), typeof(String), typeof(int) });
                                for (int i = 0; i < temp.Count; i++)
                                {
                                    ConsoleOut(temp[i]);
                                }

                                ConsoleOut("=====================");
                                ConsoleOut("");
                            }
                        }
                        else
                        {
                            code = 1;
                        }
                        break;
                    case "copy":
                        if (args.Length == 2)
                        {
                            if (Analyzer.ContainsKey((String)args[1]))
                            {
                                ExMethod(Analyzer[(String)args[1]], "AnalyzeLog", new object[] { }, new Type[] { });

                                List<String> temp = new List<String>();
                                temp = (List<String>)ExMethod(Analyzer[(String)args[1]], "Show", new object[] { baseSettings["dmg_template"], baseSettings["sort_by"], Convert.ToInt32(baseSettings["show_max"]) }, new Type[] { typeof(String), typeof(String), typeof(int) });
                                String dmgData = "| ";
                                for (int i = 0; i < temp.Count; i++)
                                {
                                    dmgData += temp[i] + " | ";
                                }

                                Thread thread = new Thread(() => Clipboard.SetText(dmgData));
                                thread.SetApartmentState(ApartmentState.STA);
                                thread.Start();
                                thread.Join();
                            }
                        }
                        else
                        {
                            code = 1;
                        }
                        break;
                    default:
                        code = 1;
                        break;
                }
            }

            return code;
        }

        public override Dictionary<string, string> CInfo()
        {
            Dictionary<String, String> info = new Dictionary<String, String>();
            info.Add("name", "Damage");
            info.Add("version", "1.0.0");
            info.Add("command", "dmg");
            info.Add("i1", "Unknown argument or arguments. Try 'help dmg' for help.");
            info.Add("i2", "Pointer already exists.");
            info.Add("i3", "Pointer does not exists.");

            return info;
        }

        public override string CHelp()
        {
            return "For handling damage information.\n\n"
                    + "Syntax:\n"
                    + "```console\n"
                    + "dmg <add | list | clear | remove | show | copy> [pointer name]\n"
                    + "```\n"
                    + "```console\n"
                    + "dmg add <pointer name>\n"
                    + "dmg list\n"
                    + "dmg clear <pointer name>\n"
                    + "dmg remove <pointer name>\n"
                    + "dmg show <pointer name>\n"
                    + "dmg copy <pointer name>\n"
                    + "```\n\n"
                    + "Arguments:\n"
                    + "-`add`: add a pointer(start point) for the analyzing with the given name `<pointer name>`\n"
                    + "-`list`: shows all pointer.\n"
                    + "-`clear`: reset a given pointer\n"
                    + "-`remove`: remove a given pointer\n"
                    + "-`show`: Shows damage information of a given pointer\n"
                    + "-`copy`: Copy damage information of a given pointer\n"
                    + "-`pointer name`: the pointer name\n\n"
                    + "Example:\n"
                    + "```console\n"
                    + "dmg add boss\n"
                    + "dmg list\n"
                    + "dmg clear boss\n"
                    + "dmg remove boss\n"
                    + "dmg show boss\n"
                    + "dmg copy boss\n"
                    + "```";
        }

        public override string CHelp(bool smallInfo)
        {
            String info = "provides damage information";

            if (!smallInfo)
            {
                info = CHelp();
            }

            return info;
        }
    }
}
