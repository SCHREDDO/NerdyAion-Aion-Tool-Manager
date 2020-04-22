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
using System.Net;
using System.Text.RegularExpressions;

namespace NerdyConsole.Commands
{
    public class Info : BaseCommand
    {
        public override int Initialize(Dictionary<string, string> baseSettings, Dictionary<string, string> commandSettings)
        {
            Obj("nerdyINI", "NerdyINI.NerdyINI", new object[] { AppDomain.CurrentDomain.BaseDirectory + "nerdy.ini" }, CPatch("lib", "NerdyINI.dll"));

            if (baseSettings["check_version"] == "1")
            {
                try
                {
                    String versionactive = ExMethod("nerdyINI", "GetVariable", new object[] { "version", "info" }, new Type[] { typeof(String), typeof(String) }).ToString();
                    const string LatestReleases = "https://api.github.com/repos/SCHREDDO/NerdyAion-Aion-Tool-Manager/releases/latest";
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    WebClient webClient = new WebClient();
                    webClient.Headers.Add("User-Agent", "Unity web player");
                    Uri uri = new Uri(LatestReleases);
                    string releaseInfo = webClient.DownloadString(uri);
                    var ma = Regex.Match(releaseInfo, @".*""tag_name"":""(.*?)"".*");
                    String versionNew = ma.Groups[1].Value;

                    Boolean available = false;

                    String[] temp1 = versionNew.Replace("v", "").Split(new char[] { '.', '-' });
                    String[] temp2 = versionactive.Replace("v", "").Split(new char[] { '.', '-' });

                    if (temp1.Length > 2)
                    {
                        if (Convert.ToInt32(temp1[0]) >= Convert.ToInt32(temp2[0]))
                        {
                            if (Convert.ToInt32(temp1[0]) > Convert.ToInt32(temp2[0]))
                            {
                                available = true;
                            }
                            else if (Convert.ToInt32(temp1[1]) > Convert.ToInt32(temp2[1]))
                            {
                                available = true;
                            }
                            else if (Convert.ToInt32(temp1[2]) > Convert.ToInt32(temp2[2]))
                            {
                                available = true;
                            }
                        }
                    }

                    if (available)
                    {
                        ConsoleOut("===============================");
                        ConsoleOut("");
                        ConsoleOut("New version available. [v" + versionactive + "] >>> [" + versionNew + "]");
                        ConsoleOut("");
                        ConsoleOut("===============================");
                        ConsoleOut("");

                        //https://github.com/SCHREDDO/NerdyAion-Aion-Tool-Manager/releases
                        //https://github.com/SCHREDDO/NerdyAion-Aion-Tool-Manager/issues/new/choose
                        if (baseSettings["new_version_browser"] == "1")
                        {
                            System.Diagnostics.Process.Start("https://github.com/SCHREDDO/NerdyAion-Aion-Tool-Manager/releases");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return 0;
        }

        public override int Execute(string command, object[] args, Dictionary<string, string> baseSettings, Dictionary<string, string> commandSettings)
        {
            int code = 0;

            if (args.Length == 0)
            {
                ConsoleOut("###################################################################");
                ConsoleOut($"{ExMethod("nerdyINI", "GetVariable", new object[] { "name", "info" }, new Type[] { typeof(String), typeof(String) })} [{ExMethod("nerdyINI", "GetVariable", new object[] { "version", "info" }, new Type[] { typeof(String), typeof(String) })}]");
                ConsoleOut($"Author: {ExMethod("nerdyINI", "GetVariable", new object[] { "author", "info" }, new Type[] { typeof(String), typeof(String) })}");
                ConsoleOut($"Repository: {ExMethod("nerdyINI", "GetVariable", new object[] { "repository", "info" }, new Type[] { typeof(String), typeof(String) })}");
                ConsoleOut("###################################################################");

                try
                {
                    String versionactive = (String)ExMethod("nerdyINI", "GetVariable", new object[] { "version", "info" }, new Type[] { typeof(String), typeof(String) });
                    const string LatestReleases = "https://api.github.com/repos/SCHREDDO/NerdyAion-Aion-Tool-Manager/releases/latest";
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    WebClient webClient = new WebClient();
                    webClient.Headers.Add("User-Agent", "Unity web player");
                    Uri uri = new Uri(LatestReleases);
                    string releaseInfo = webClient.DownloadString(uri);
                    var m = Regex.Match(releaseInfo, @".*""tag_name"":""(.*?)"".*");
                    String versionNew = m.Groups[1].Value;

                    if (!versionNew.Equals(versionactive))
                    {
                        Boolean available = false;

                        String[] temp1 = versionNew.Replace("v", "").Split(new char[] { '.', '-' });
                        String[] temp2 = versionactive.Replace("v", "").Split(new char[] { '.', '-' });

                        if (temp1.Length > 2)
                        {
                            if (Convert.ToInt32(temp1[0]) >= Convert.ToInt32(temp2[0]))
                            {
                                if (Convert.ToInt32(temp1[0]) > Convert.ToInt32(temp2[0]))
                                {
                                    available = true;
                                }
                                else if (Convert.ToInt32(temp1[1]) > Convert.ToInt32(temp2[1]))
                                {
                                    available = true;
                                }
                                else if (Convert.ToInt32(temp1[2]) > Convert.ToInt32(temp2[2]))
                                {
                                    available = true;
                                }
                            }
                        }

                        if (available)
                        {
                            ConsoleOut("");
                            ConsoleOut("New version available. [v" + versionactive + "] >>> [" + versionNew + "]");

                            if (baseSettings["new_version_browser"] == "1")
                            {
                                System.Diagnostics.Process.Start("https://github.com/SCHREDDO/NerdyAion-Aion-Tool-Manager/releases");
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    ConsoleOut(e.ToString());
                }
            }
            else if (args.Length == 1)
            {
                switch (args[0].ToString())
                {
                    case "-a":
                        ConsoleOut(ExMethod("nerdyINI", "GetVariable", new object[] { "author", "info" }, new Type[] { typeof(String), typeof(String) }).ToString());
                        break;
                    case "-r":
                        ConsoleOut(ExMethod("nerdyINI", "GetVariable", new object[] { "repository", "info" }, new Type[] { typeof(String), typeof(String) }).ToString());
                        break;
                    case "-v":
                        ConsoleOut(ExMethod("nerdyINI", "GetVariable", new object[] { "version", "info" }, new Type[] { typeof(String), typeof(String) }).ToString());
                        break;
                    case "-c":
                        try
                        {
                            String versionactive = (String)ExMethod("nerdyINI", "GetVariable", new object[] { "version", "info" }, new Type[] { typeof(String), typeof(String) });
                            const string LatestReleases = "https://api.github.com/repos/SCHREDDO/NerdyAion-Aion-Tool-Manager/releases/latest";
                            ServicePointManager.Expect100Continue = true;
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                            WebClient webClient = new WebClient();
                            webClient.Headers.Add("User-Agent", "Unity web player");
                            Uri uri = new Uri(LatestReleases);
                            string releaseInfo = webClient.DownloadString(uri);
                            var m = Regex.Match(releaseInfo, @".*""tag_name"":""(.*?)"".*");
                            String versionNew = m.Groups[1].Value;

                            Boolean available = false;

                            String[] temp1 = versionNew.Replace("v", "").Split(new char[] { '.', '-' });
                            String[] temp2 = versionactive.Replace("v", "").Split(new char[] { '.', '-' });

                            if (temp1.Length > 2)
                            {
                                if (Convert.ToInt32(temp1[0]) >= Convert.ToInt32(temp2[0]))
                                {
                                    if (Convert.ToInt32(temp1[0]) > Convert.ToInt32(temp2[0]))
                                    {
                                        available = true;
                                    }
                                    else if (Convert.ToInt32(temp1[1]) > Convert.ToInt32(temp2[1]))
                                    {
                                        available = true;
                                    }
                                    else if (Convert.ToInt32(temp1[2]) > Convert.ToInt32(temp2[2]))
                                    {
                                        available = true;
                                    }
                                }
                            }

                            if (available)
                            {
                                ConsoleOut("New version available. [v" + versionactive + "] >>> [" + versionNew + "]");

                                if (baseSettings["new_version_browser"] == "1")
                                {
                                    System.Diagnostics.Process.Start("https://github.com/SCHREDDO/NerdyAion-Aion-Tool-Manager/releases");
                                }
                            }
                            else
                            {
                                ConsoleOut("latest version");
                            }
                        }
                        catch (Exception e)
                        {
                            ConsoleOut(e.ToString());
                        }
                        break;
                    default:
                        code = 1;
                        break;
                }
            }
            else
            {
                code = 1;
            }

            if (code == 0)
            {
                ConsoleOut("");
            }

            return code;
        }

        public override Dictionary<string, string> CInfo()
        {
            Dictionary<String, String> info = new Dictionary<String, String>();
            info.Add("name", "Info");
            info.Add("version", "1.0.0");
            info.Add("command", "info");
            info.Add("i1", "Unknown argument or arguments. Try 'help info' for help.");

            return info;
        }

        public override string CHelp()
        {
            return "Displayed information about the used application.\n\n"
                    + "Syntax:\n"
                    + "``console\n"
                    + "info [-a | -r | -v | -c]\n"
                    + "```\n\n"
                    + "Arguments:\n"
                    + "-`-a`: shows only the author of the application\n"
                    + "-`-r`: shows only the repository of the application\n"
                    + "-`-v`: shows only the version of the application\n"
                    + "-`-c`: checks if there is a new version available\n\n"
                    + "Example:\n"
                    + "```console\n"
                    + "info\n"
                    + "info - v\n"
                    + "```";
        }

        public override string CHelp(bool smallInfo)
        {
            String info = "displayed information about the used application";

            if (!smallInfo)
            {
                info = CHelp();
            }

            return info;
        }
    }
}
