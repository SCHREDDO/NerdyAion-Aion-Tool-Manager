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
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;

namespace NerdyDamageAnalyzer
{
    public class NerdyDamageAnalyzer
    {
        private String playerName;
        private Dictionary<String, Type> TypeList { get; set; }
        private Dictionary<String, Object> ObjectList { get; set; }

        public List<AnalysisTemplate> AnalysisTemplates { get; set; }

        public Dictionary<String, Player> PlayerList { get; set; }
        public Dictionary<String, String> SkillList { get; set; }

        public NerdyDamageAnalyzer(String logFilePath, String languarge = "DE", String playerName = "ME")
        {
            TypeList = new Dictionary<string, Type>();
            ObjectList = new Dictionary<string, object>();

            this.playerName = playerName;
            Obj("logReader", "NerdyLogReader.NerdyLogReader", CPatch("lib", "NerdyLogReader.dll"), new object[] { logFilePath }, true);

            AnalysisTemplates = new List<AnalysisTemplate>();
            PlayerList = new Dictionary<string, Player>();
            SkillList = new Dictionary<string, string>();

            SetAnalysisTemplates(languarge);
        }

        private Timer timer1 = null;
        public void InitTimer()
        {
            timer1 = new Timer();
            timer1.Elapsed += (sender, args) => AnalyzeLog();
            timer1.AutoReset = true;
            timer1.Interval = 1000;
            timer1.Start();
        }

        public void AnalyzeLog()
        {
            //byte[] logData = Log.ReadLogChanges();
            byte[] logData = (byte[])ExMethod("logReader", "ReadLogChanges", new object[] { }, new Type[] { });
            String[] lines = new String[0];
            String[] stringSeparators = new String[] { "\r\n" };
            Regex pattern = null;
            Match match = null;
            String logText = "";

            Boolean isCriticalHit = false;
            String eventSource = "";
            String eventTarget = "";
            DateTime actionTime = new DateTime();
            String time = "";
            String eventName = "";
            long eventEffectDamage = 0;

            logText = ByteArrayToString(logData);
            logData = new byte[0];
            lines = logText.Split(stringSeparators, StringSplitOptions.None);
            logText = "";

            foreach (String temp in lines)
            {
                String line = temp.Replace("\r\n", "");
                foreach (AnalysisTemplate template in AnalysisTemplates)
                {
                    if (Regex.IsMatch(line, template.Strucktor))
                    {
                        pattern = new Regex(template.Template);
                        match = pattern.Match(line);

                        foreach (KeyValuePair<String, String> variable in template.Variables)
                        {
                            switch (variable.Value)
                            {
                                case "eventSource":
                                    eventSource = template.GetEventSource(match);
                                    break;
                                case "eventTarget":
                                    eventTarget = template.GetEventTarget(match);
                                    break;
                                case "eventEffect":
                                    if (template.DamageEvent)
                                    {
                                        eventEffectDamage = template.GetEventEffect(match);
                                    }
                                    break;
                                case "eventName":
                                    eventName = template.GetEventName(match);
                                    break;
                                case "eventEffectExtra":
                                    break;
                                case "eventTarget_eventEffect":
                                    eventTarget = template.GetEventTarget(match);
                                    if (template.DamageEvent)
                                    {
                                        eventEffectDamage = template.GetEventEffect(match);
                                    }
                                    break;
                                case "eventName_eventEffect":
                                    eventName = template.GetEventName(match);
                                    if (template.DamageEvent)
                                    {
                                        eventEffectDamage = template.GetEventEffect(match);
                                    }
                                    break;
                                case "eventSource_eventName":
                                    eventSource = template.GetEventSource(match);
                                    eventName = template.GetEventName(match);
                                    if (template.DamageEvent)
                                    {
                                        eventEffectDamage = template.GetEventEffect(match);
                                    }
                                    break;
                                case "time":
                                    time = template.GetTime(match);
                                    break;
                                default:
                                    break;
                            }
                        }

                        isCriticalHit = template.CheckForCriticalHit(line);

                        break;
                    }
                }

                if (eventSource != "")
                {
                    if (!PlayerList.ContainsKey(eventSource))
                    {
                        PlayerList.Add(eventSource, new Player(eventSource, this.playerName));
                    }

                    actionTime = DateTime.ParseExact(time, "yyyy.MM.dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

                    PlayerList[eventSource].AddSkill(new Skill(eventName, eventTarget, actionTime, eventEffectDamage, isCriticalHit));

                    if (eventName != "")
                    {
                        if (!SkillList.ContainsKey((eventName + "_" + eventTarget)))
                        {
                            SkillList.Add((eventName + "_" + eventTarget), "");
                        }

                        SkillList[(eventName + "_" + eventTarget)] = eventSource;
                    }
                }
                else if (SkillList.ContainsKey((eventName + "_" + eventTarget)))
                {
                    if (PlayerList.ContainsKey(SkillList[(eventName + "_" + eventTarget)]))
                    {
                        PlayerList[SkillList[(eventName + "_" + eventTarget)]].AddSkillTick(eventName, eventTarget, eventEffectDamage, actionTime);
                    }
                }

                eventSource = "";
                eventTarget = "";
                time = "";
                eventName = "";
                eventEffectDamage = 0;
            }
        }

        public void Clear()
        {
            ExMethod("logReader", "ReadLogChanges", new object[] { }, new Type[] { });

            PlayerList = new Dictionary<string, Player>();
            SkillList = new Dictionary<string, string>();
        }

        public List<String> Show(String template, String orderBy, int max)
        {
            List<String> list = new List<String>();
            List<Player> playerList = PlayerList.Values.ToList();

            switch (orderBy)
            {
                case "name":
                    playerList.OrderBy(x => x.Name);
                    break;
                case "dmg":
                    playerList.OrderByDescending(x => x.Damge);
                    break;
                case "dot_dmg":
                    playerList.OrderByDescending(x => x.DotDamge);
                    break;
                case "crit_dmg":
                    playerList.OrderByDescending(x => x.CritDamge);
                    break;
                case "dps":
                    playerList.OrderByDescending(x => x.GetDPS(false));
                    break;
                case "action_time":
                    playerList.OrderByDescending(x => x.ActionTime);
                    break;
                case "skill_count":
                    playerList.OrderByDescending(x => x.UsedSkills.Count);
                    break;
            }

            if (max == 0)
            {
                max = playerList.Count;
            }

            if (max > playerList.Count)
            {
                max = playerList.Count;
            }

            for (int i = 0; i < max; i++)
            {
                list.Add(new StringBuilder(template)
                            .Replace("<player>", playerList[i].Name)
                            .Replace("<dmg>", playerList[i].Damge.ToString())
                            .Replace("<dps>", playerList[i].GetDPS(false).ToString())
                            .Replace("<dot_dmg>", playerList[i].DotDamge.ToString())
                            .Replace("<crit_dmg>", playerList[i].CritDamge.ToString())
                            .Replace("<used_skills_count>", playerList[i].UsedSkills.Count.ToString())
                            .Replace("<time>", playerList[i].ActionTime.ToString()).ToString());
            }

            return list;
        }

        private String ByteArrayToString(byte[] data)
        {
            String text = "";

            //new String(data)
            //System.Text.Encoding.Default.GetString(data);
            //System.Text.Encoding.UTF8.GetString(data);

            foreach (byte item in data)
            {
                text += (char)item;
            }

            return text;
        }

        private void SetAnalysisTemplates(String languarge)
        {
            switch (languarge.ToUpper())
            {
                case "DE":
                    AnalysisTemplatesDE();
                    break;
                case "EN":
                    AnalysisTemplatesEN();
                    break;
                default:
                    AnalysisTemplatesEN();
                    break;
            }
        }

        private void Obj(String indexName, String objName, String path)
        {
            this.Obj(indexName, objName, path, new object[] { }, true);
        }

        private void Obj(String indexName, String objName, object[] parameter, String path)
        {
            this.Obj(indexName, objName, path, parameter, true);
        }

        private void Obj(String indexName, String objName, String path, Boolean obj)
        {
            this.Obj(indexName, objName, path, new object[] { }, obj);
        }

        private void Obj(String indexName, String objName, String path, object[] parameter, Boolean obj)
        {
            TypeList.Add(indexName, Assembly.LoadFrom(path).GetType(objName));

            if (obj)
            {
                ObjectList.Add(indexName, Activator.CreateInstance(TypeList[indexName], parameter));
            }
        }

        private object ExMethod(String indexName, String method, object[] parameter, Type[] parameterType)
        {
            Object obj = null;
            if (ObjectList.ContainsKey(indexName))
            {
                obj = ObjectList[indexName];
            }

            return TypeList[indexName].GetMethod(method, parameterType).Invoke(obj, parameter);
        }

        private String CPatch(String path, String dll)
        {
            return this.CPatch(new String[] { path }, dll);
        }

        private String CPatch(String[] path, String dll)
        {
            return $"{AppDomain.CurrentDomain.BaseDirectory}{Path.Combine(path)}{Path.DirectorySeparatorChar}{dll}";
        }

        private void AnalysisTemplatesDE()
        {
            AnalysisTemplate temp = null;

            //2019.03.16 17:09:04 : Ihr habt Wilder Sumpf-Oculis durch Benutzung von Seelenraub 24.876 Schaden zugefügt.
            temp = new AnalysisTemplate();
            temp.DamageEvent = true;
            temp.DamageOverTime = false;
            temp.CriticalHit = false;
            temp.Strucktor = ".* : .* habt .* durch Benutzung von .* Schaden zugefügt.";
            temp.Template = @"(?<time>[^,]+) : (?<eventSource>[^,]+) habt (?<eventTarget>[^,]+) durch Benutzung von (?<eventName_eventEffect>[^,]+) Schaden zugefügt.";
            temp.CriticalIdentifier = "Kritischer Treffer!";
            temp.AddVariable("time");
            temp.AddVariable("eventSource");
            temp.AddVariable("eventTarget");
            temp.AddVariable("eventName_eventEffect");

            AnalysisTemplates.Add(temp);

            //2019.03.16 17:09:13 : Ihr fügt Vom Drachenbund ausgebildeter Mantikor durch Magische Umkehr 0 Schaden zu und hebt einige der magischen Verstärkungen und Schwächungen auf. 
            temp = new AnalysisTemplate();
            temp.DamageEvent = true;
            temp.DamageOverTime = false;
            temp.CriticalHit = false;
            temp.Strucktor = ".* : .* fügt .* durch .* Schaden zu und .*.";
            temp.Template = @"(?<time>[^,]+) : (?<eventSource>[^,]+) fügt (?<eventTarget>[^,]+) durch (?<eventName_eventEffect>[^,]+) Schaden zu und (?<eventEffectExtra>[^,]+).";
            temp.CriticalIdentifier = "Kritischer Treffer!";
            temp.AddVariable("time");
            temp.AddVariable("eventSource");
            temp.AddVariable("eventTarget");
            temp.AddVariable("eventName_eventEffect");
            temp.AddVariable("eventEffectExtra");

            AnalysisTemplates.Add(temp);

            //2019.03.16 17:09:14 : Ihr fügt Vom Drachenbund ausgebildeter Mantikor durch Mana-Explosion fortwährend Schaden zu.
            temp = new AnalysisTemplate();
            temp.DamageEvent = true;
            temp.DamageOverTime = false;
            temp.CriticalHit = false;
            temp.Strucktor = ".* : .* fügt .* durch .* fortwährend Schaden zu.";
            temp.Template = @"(?<time>[^,]+) : (?<eventSource>[^,]+) fügt (?<eventTarget>[^,]+) durch (?<eventName>[^,]+) fortwährend Schaden zu.";
            temp.CriticalIdentifier = "Kritischer Treffer!";
            temp.AddVariable("time");
            temp.AddVariable("eventSource");
            temp.AddVariable("eventTarget");
            temp.AddVariable("eventName");

            AnalysisTemplates.Add(temp);

            //2019.03.16 17:09:15 : Vom Drachenbund ausgebildeter Mantikor hat Euch durch Niederwerfen 0 Schaden zugefügt.
            temp = new AnalysisTemplate();
            temp.DamageEvent = true;
            temp.DamageOverTime = false;
            temp.CriticalHit = false;
            temp.Strucktor = ".* : .* hat .* durch .* Schaden zugefügt.";
            temp.Template = @"(?<time>[^,]+) : (?<eventSource>[^,]+) hat (?<eventTarget>[^,]+) durch (?<eventName_eventEffect>[^,]+) Schaden zugefügt.";
            temp.CriticalIdentifier = "Kritischer Treffer!";
            temp.AddVariable("time");
            temp.AddVariable("eventSource");
            temp.AddVariable("eventTarget");
            temp.AddVariable("eventName_eventEffect");

            AnalysisTemplates.Add(temp);

            //2019.03.16 17:09:18 : Vom Drachenbund ausgebildeter Mantikor erhält durch Erosion 6.736 Schaden.
            temp = new AnalysisTemplate();
            temp.DamageEvent = true;
            temp.DamageOverTime = false;
            temp.CriticalHit = false;
            temp.Strucktor = ".* : .* erhält durch .* Schaden.";
            temp.Template = @"(?<time>[^,]+) : (?<eventTarget>[^,]+) erhält durch (?<eventName_eventEffect>[^,]+) Schaden.";
            temp.CriticalIdentifier = "Kritischer Treffer!";
            temp.AddVariable("time");
            temp.AddVariable("eventTarget");
            temp.AddVariable("eventName_eventEffect");

            AnalysisTemplates.Add(temp);

            //2019.03.16 17:09:57 : Ihr habt Derbfell-Rynoce 7.572 Schaden zugefügt.
            temp = new AnalysisTemplate();
            temp.DamageEvent = true;
            temp.DamageOverTime = false;
            temp.CriticalHit = false;
            temp.Strucktor = ".* : .* habt .* Schaden zugefügt.";
            temp.Template = @"(?<time>[^,]+) : (?<eventSource>[^,]+) habt (?<eventTarget_eventEffect>[^,]+) Schaden zugefügt.";
            temp.CriticalIdentifier = "Kritischer Treffer!";
            temp.AddVariable("time");
            temp.AddVariable("eventSource");
            temp.AddVariable("eventTarget_eventEffect");

            AnalysisTemplates.Add(temp);

            //2019.03.16 17:10:33 : Aikumi hat Boshafter Excura durch Benutzung von Seelenraub 24.360 Schaden zugefügt.
            temp = new AnalysisTemplate();
            temp.DamageEvent = true;
            temp.DamageOverTime = false;
            temp.CriticalHit = false;
            temp.Strucktor = ".* : .* hat .* durch Benutzung von .* Schaden zugefügt.";
            temp.Template = @"(?<time>[^,]+) : (?<eventSource>[^,]+) hat (?<eventTarget>[^,]+) durch Benutzung von (?<eventName_eventEffect>[^,]+) Schaden zugefügt.";
            temp.CriticalIdentifier = "Kritischer Treffer!";
            temp.AddVariable("time");
            temp.AddVariable("eventSource");
            temp.AddVariable("eventTarget");
            temp.AddVariable("eventName_eventEffect");

            AnalysisTemplates.Add(temp);

            //2019.03.16 17:19:16 : Piton hat Sherilla-Antri 721 Schaden zugefügt.
            temp = new AnalysisTemplate();
            temp.DamageEvent = true;
            temp.DamageOverTime = false;
            temp.CriticalHit = false;
            temp.Strucktor = ".* : .* hat .* Schaden zugefügt.";
            temp.Template = @"(?<time>[^,]+) : (?<eventSource>[^,]+) hat (?<eventTarget_eventEffect>[^,]+) Schaden zugefügt.";
            temp.CriticalIdentifier = "Kritischer Treffer!";
            temp.AddVariable("time");
            temp.AddVariable("eventSource");
            temp.AddVariable("eventTarget_eventEffect");

            AnalysisTemplates.Add(temp);

            temp = new AnalysisTemplate();
            temp.DamageEvent = true;
            temp.DamageOverTime = false;
            temp.CriticalHit = false;
            temp.Strucktor = ".* : .* habt .* durch .* Schaden zugefügt.";
            temp.Template = @"(?<time>[^,]+) : (?<eventSource>[^,]+) habt (?<eventTarget>[^,]+) durch (?<eventName_eventEffect>[^,]+) Schaden zugefügt.";
            temp.CriticalIdentifier = "Kritischer Treffer!";
            temp.AddVariable("time");
            temp.AddVariable("eventSource");
            temp.AddVariable("eventTarget");
            temp.AddVariable("eventName_eventEffect");

            AnalysisTemplates.Add(temp);

            //2019.03.16 17:18:53 : Flammen - Piton erhält durch Umfangreiche Erosion 7.278 Schaden.
            temp = new AnalysisTemplate();
            temp.DamageEvent = true;
            temp.DamageOverTime = true;
            temp.CriticalHit = false;
            temp.Strucktor = ".* : .* erhält durch .* Schaden.";
            temp.Template = @"(?<time>[^,]+) : (?<eventTarget>[^,]+) erhält durch (?<eventName_eventEffect>[^,]+) Schaden.";
            temp.CriticalIdentifier = "Kritischer Treffer!";
            temp.AddVariable("time");
            temp.AddVariable("eventTarget");
            temp.AddVariable("eventName_eventEffect");

            AnalysisTemplates.Add(temp);

            //2019.03.16 17:18:40 : Ihr fügt Flammen-Piton durch Höllenqualen fortwährend Schaden zu. 
            temp = new AnalysisTemplate();
            temp.DamageEvent = false;
            temp.DamageOverTime = true;
            temp.CriticalHit = false;
            temp.Strucktor = ".* : .* fügt .* durch .* fortwährend Schaden zu.";
            temp.Template = @"(?<time>[^,]+) : (?<eventSource>[^,]+) fügt (?<eventTarget>[^,]+) durch (?<eventName>[^,]+) fortwährend Schaden zu.";
            temp.CriticalIdentifier = "Kritischer Treffer!";
            temp.AddVariable("time");
            temp.AddVariable("eventSource");
            temp.AddVariable("eventTarget");
            temp.AddVariable("eventName");

            AnalysisTemplates.Add(temp);

            //2019.03.24 00:38:48 : Prüfungsvogelscheuche erhält den Effekt 'Verzögerte Explosion', weil Ihr Großer Vulkanausbruch benutzt habt.
            temp = new AnalysisTemplate();
            temp.DamageEvent = false;
            temp.DamageOverTime = true;
            temp.CriticalHit = false;
            temp.Strucktor = ".* : .* erhält den Effekt '.*', weil .* benutzt habt.";
            temp.Template = @"(?<time>[^,]+) : (?<eventTarget>[^,]+) erhält den Effekt '(?<effect>[^,]+)', weil (?<eventSource_eventName>[^,]+) benutzt habt.";
            temp.CriticalIdentifier = "Kritischer Treffer!";
            temp.AddVariable("time");
            temp.AddVariable("eventTarget");
            temp.AddVariable("eventSource_eventName");

            AnalysisTemplates.Add(temp);

            temp = new AnalysisTemplate();
            temp.DamageEvent = false;
            temp.DamageOverTime = true;
            temp.CriticalHit = false;
            temp.Strucktor = ".* : .* erhält den Effekt '.*', weil .* benutzt hat.";
            temp.Template = @"(?<time>[^,]+) : (?<eventTarget>[^,]+) erhält den Effekt '(?<effect>[^,]+)', weil (?<eventSource_eventName>[^,]+) benutzt hat.";
            temp.CriticalIdentifier = "Kritischer Treffer!";
            temp.AddVariable("time");
            temp.AddVariable("eventTarget");
            temp.AddVariable("eventSource_eventName");

            AnalysisTemplates.Add(temp);
        }

        private void AnalysisTemplatesEN()
        {
            AnalysisTemplate temp = null;

            //2019.03.26 18:01:35 : You received 113 damage from Poisonous Piton. 
            temp = new AnalysisTemplate();
            temp.DamageEvent = true;
            temp.DamageOverTime = false;
            temp.CriticalHit = false;
            temp.Strucktor = ".* : .* received .* damage from .*.";
            temp.Template = @"(?<time>[^,]+) : (?<eventTarget>[^,]+) received (?<eventEffect>[^,]+) damage from (?<eventSource>[^,]+).";
            temp.CriticalIdentifier = "Critical Strike!";
            temp.AddVariable("time");
            temp.AddVariable("eventTarget");
            temp.AddVariable("eventEffect");
            temp.AddVariable("eventSource");

            AnalysisTemplates.Add(temp);

            //2019.03.26 18:01:36 : You inflicted 0 damage on Flaming Piton and dispelled some of their magical buffs and debuffs by using Magic Explosion. 
            temp = new AnalysisTemplate();
            temp.DamageEvent = true;
            temp.DamageOverTime = false;
            temp.CriticalHit = false;
            temp.Strucktor = ".* : .* inflicted .* damage on .* and .* by using .*.";
            temp.Template = @"(?<time>[^,]+) : (?<eventSource>[^,]+) inflicted (?<eventEffect>[^,]+) damage on (?<eventTarget>[^,]+) and (?<eventEffectExtra>[^,]+) by using (?<eventName>[^,]+).";
            temp.CriticalIdentifier = "Critical Strike!";
            temp.AddVariable("time");
            temp.AddVariable("eventSource");
            temp.AddVariable("eventEffect");
            temp.AddVariable("eventTarget");
            temp.AddVariable("eventEffectExtra");
            temp.AddVariable("eventName");

            AnalysisTemplates.Add(temp);

            //2019.03.26 18:01:37 : Flaming Piton has inflicted 2.207 damage on you by using Destructive Strike. 
            temp = new AnalysisTemplate();
            temp.DamageEvent = true;
            temp.DamageOverTime = false;
            temp.CriticalHit = false;
            temp.Strucktor = ".* : .* has inflicted .* damage on .* by using .*.";
            temp.Template = @"(?<time>[^,]+) : (?<eventSource>[^,]+) has inflicted (?<eventEffect>[^,]+) damage on (?<eventTarget>[^,]+) by using (?<eventName>[^,]+).";
            temp.CriticalIdentifier = "Critical Strike!";
            temp.AddVariable("time");
            temp.AddVariable("eventSource");
            temp.AddVariable("eventEffect");
            temp.AddVariable("eventTarget");
            temp.AddVariable("eventName");

            AnalysisTemplates.Add(temp);

            //2019.03.26 18:01:45 : You inflicted 26.150 damage on Flaming Piton by using Cyclone of Wrath.
            temp = new AnalysisTemplate();
            temp.DamageEvent = true;
            temp.DamageOverTime = false;
            temp.CriticalHit = false;
            temp.Strucktor = ".* : .* inflicted .* damage on .* by using .*.";
            temp.Template = @"(?<time>[^,]+) : (?<eventSource>[^,]+) inflicted (?<eventEffect>[^,]+) damage on (?<eventTarget>[^,]+) by using (?<eventName>[^,]+).";
            temp.CriticalIdentifier = "Critical Strike!";
            temp.AddVariable("time");
            temp.AddVariable("eventSource");
            temp.AddVariable("eventEffect");
            temp.AddVariable("eventTarget");
            temp.AddVariable("eventName");

            AnalysisTemplates.Add(temp);

            //2019.03.26 18:01:46 : Flaming Piton received 9.282 damage due to the effect of Erosion.
            temp = new AnalysisTemplate();
            temp.DamageEvent = true;
            temp.DamageOverTime = true;
            temp.CriticalHit = false;
            temp.Strucktor = ".* : .* received .* damage due to the effect of .*.";
            temp.Template = @"(?<time>[^,]+) : (?<eventTarget>[^,]+) received (?<eventEffect>[^,]+) damage due to the effect of (?<eventName>[^,]+).";
            temp.CriticalIdentifier = "Critical Strike!";
            temp.AddVariable("time");
            temp.AddVariable("eventTarget");
            temp.AddVariable("eventEffect");
            temp.AddVariable("eventName");

            AnalysisTemplates.Add(temp);

            //2019.03.26 18:01:39 : You inflicted continuous damage on Flaming Piton by using Erosion. 
            temp = new AnalysisTemplate();
            temp.DamageEvent = false;
            temp.DamageOverTime = true;
            temp.CriticalHit = false;
            temp.Strucktor = ".* : .* inflicted continuous damage on .* by using .*.";
            temp.Template = @"(?<time>[^,]+) : (?<eventSource>[^,]+) inflicted continuous damage on (?<eventTarget>[^,]+) by using (?<eventName>[^,]+).";
            temp.CriticalIdentifier = "Critical Strike!";
            temp.AddVariable("time");
            temp.AddVariable("eventSource");
            temp.AddVariable("eventTarget");
            temp.AddVariable("eventName");

            AnalysisTemplates.Add(temp);

            //2019.03.26 18:01:54 : Vrajitoarea-Nerga used Flame Cage to inflict the continuous damage effect on Poisonous Piton.
            temp = new AnalysisTemplate();
            temp.DamageEvent = false;
            temp.DamageOverTime = true;
            temp.CriticalHit = false;
            temp.Strucktor = ".* : .* used .* to inflict the continuous damage effect on .*.";
            temp.Template = @"(?<time>[^,]+) : (?<eventSource>[^,]+) used (?<eventName>[^,]+) to inflict the continuous damage effect on (?<eventTarget>[^,]+).";
            temp.CriticalIdentifier = "Critical Strike!";
            temp.AddVariable("time");
            temp.AddVariable("eventSource");
            temp.AddVariable("eventName");
            temp.AddVariable("eventTarget");

            AnalysisTemplates.Add(temp);

            //2019.03.26 18:01:54 : Exam Scarecrow received the Delayed Blast effect as you used Big Magma Eruption.
            temp = new AnalysisTemplate();
            temp.DamageEvent = false;
            temp.DamageOverTime = true;
            temp.CriticalHit = false;
            temp.Strucktor = ".* : .* received the .* effect as .* used .*.";
            temp.Template = @"(?<time>[^,]+) : (?<eventTarget>[^,]+) received the (?<event>[^,]+) effect as (?<eventSource>[^,]+) used (?<eventName>[^,]+).";
            temp.CriticalIdentifier = "Critical Strike!";
            temp.AddVariable("time");
            temp.AddVariable("eventTarget");
            temp.AddVariable("event");
            temp.AddVariable("eventSource");
            temp.AddVariable("eventName");

            AnalysisTemplates.Add(temp);
        }
    }
}
