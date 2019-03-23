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
// Last Edited On: 23.03.2019
// Language: C#
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace NerdyAion
{
    public class LogAnalyzer
    {
        private LogReader log;
        private List<AnalysisTemplate> analysisTemplates;
        
        private Dictionary<String, Player> playerList;
        private Dictionary<String, String> skillList;

        public LogReader Log
        {
            get { return log; }
            set { log = value; }
        }
        public List<AnalysisTemplate> AnalysisTemplates
        {
            get { return analysisTemplates; }
            set { analysisTemplates = value; }
        }

        public Dictionary<String, Player> PlayerList
        {
            get { return playerList; }
            set { playerList = value; }
        }
        public Dictionary<String, String> SkillList
        {
            get { return skillList; }
            set { skillList = value; }
        }

        public LogAnalyzer(String logFilePath)
        {
            Log = new LogReader(logFilePath);
            AnalysisTemplates = new List<AnalysisTemplate>();
            PlayerList = new Dictionary<string, Player>();
            SkillList = new Dictionary<string, string>();

            SetAnalysisTemplates();
        }

        public void AnalyzeLog()
        {
            byte[] logData = Log.ReadLogChanges();
            String[] lines = new String[0];
            String[] stringSeparators = new String[] { "\r\n" };
            Regex pattern = null;
            Match match = null;
            String logText = "";

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
                                case "time":
                                    time = template.GetTime(match);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    if (eventSource != "")
                    {
                        if (!PlayerList.ContainsKey(eventSource))
                        {
                            PlayerList.Add(eventSource, new Player(eventSource));
                        }

                        actionTime = DateTime.ParseExact(time, "yyyy.MM.dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

                        PlayerList[eventSource].AddSkill(new Skill(eventName, eventTarget, actionTime, eventEffectDamage, false));

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
        }

        private String ByteArrayToString(byte[] data)
        {
            String text = "";

            foreach (byte item in data)
            {
                text += (char)item;
            }

            return text;
        }

        private void SetAnalysisTemplates()
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
        }
    }
}
