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
using System.Text.RegularExpressions;

namespace NerdyDamageAnalyzer
{
    public class AnalysisTemplate
    {
        private String strucktor;
        private String template;
        private String criticalIdentifier;
        /*
         * Supported variables:
         * time
         * eventSource
         * eventTarget
         * eventEffect
         * eventName
         * eventEffectExtra
         * eventSource_eventName
         * eventTarget_eventEffect
         * eventName_eventEffect
         */
        private Dictionary<String, String> variables;
        private Boolean damageEvent;
        private Boolean damageOverTime;
        private Boolean criticalHit;

        public String Strucktor
        {
            get { return strucktor; }
            set { strucktor = value; }
        }
        public String Template
        {
            get { return template; }
            set { template = value; }
        }
        public String CriticalIdentifier
        {
            get { return criticalIdentifier; }
            set { criticalIdentifier = value; }
        }
        public Dictionary<String, String> Variables
        {
            get { return variables; }
            set { variables = value; }
        }
        public Boolean DamageEvent
        {
            get { return damageEvent; }
            set { damageEvent = value; }
        }
        public Boolean DamageOverTime
        {
            get { return damageOverTime; }
            set { damageOverTime = value; }
        }
        public Boolean CriticalHit
        {
            get { return criticalHit; }
            set { criticalHit = value; }
        }

        public AnalysisTemplate()
        {
            Strucktor = "";
            Template = "";
            CriticalIdentifier = "";
            Variables = new Dictionary<string, string>();
            DamageEvent = false;
            DamageOverTime = false;
            CriticalHit = false;
        }

        public void AddVariable(String variable)
        {
            Variables.Add(variable, variable);
        }

        public String GetTime(Match result)
        {
            return result.Groups[Variables["time"]].Value;
        }

        public String GetEventSource(Match result)
        {
            if (Variables.ContainsKey("eventSource_eventName"))
            {
                return SplitEventSourceAndEventName(result)[0];
            }

            return result.Groups[Variables["eventSource"]].Value;
        }

        public String GetEventTarget(Match result)
        {
            if (Variables.ContainsKey("eventTarget_eventEffect"))
            {
                return SplitEventTargetAndEventEffect(result)[0];
            }

            return result.Groups[Variables["eventTarget"]].Value;
        }

        public String GetEventName(Match result)
        {
            if (Variables.ContainsKey("eventName_eventEffect"))
            {
                return SplitEventNameAndEventEffect(result)[0];
            }
            else if (Variables.ContainsKey("eventSource_eventName"))
            {
                return SplitEventSourceAndEventName(result)[1];
            }

            return result.Groups[Variables["eventName"]].Value;
        }

        public long GetEventEffect(Match result)
        {
            if (Variables.ContainsKey("eventTarget_eventEffect"))
            {
                return Convert.ToInt64(SplitEventTargetAndEventEffect(result)[1].Replace(".", ""));
            }
            else if (Variables.ContainsKey("eventName_eventEffect"))
            {
                return Convert.ToInt64(SplitEventNameAndEventEffect(result)[1].Replace(".", ""));
            }

            return Convert.ToInt64(result.Groups[Variables["eventEffect"]].Value.Replace(".", ""));
        }

        public String GetEventEffectExtra(Match result)
        {
            return result.Groups[Variables["eventEffectExtra"]].Value;
        }

        private String[] SplitEventTargetAndEventEffect(Match result)
        {
            Regex pattern = new Regex(@"(?<eventTarget>[^,]+) (?<eventEffect>[.0-9]+)");
            Match match = pattern.Match(result.Groups[variables["eventTarget_eventEffect"]].Value);

            return new String[] { match.Groups["eventTarget"].Value, match.Groups["eventEffect"].Value };
        }

        private String[] SplitEventNameAndEventEffect(Match result)
        {
            Regex pattern = new Regex(@"(?<eventName>[^,]+) (?<eventEffect>[.0-9]+)");
            Match match = pattern.Match(result.Groups[variables["eventName_eventEffect"]].Value);

            return new String[] { match.Groups["eventName"].Value, match.Groups["eventEffect"].Value };
        }

        private String[] SplitEventSourceAndEventName(Match result)
        {
            Regex pattern = new Regex(@"(?<eventSource>[a-zA-Z]+) (?<eventName>[^,]+)");
            Match match = pattern.Match(result.Groups[variables["eventSource_eventName"]].Value);

            return new String[] { match.Groups["eventSource"].Value, match.Groups["eventName"].Value };
        }

        public Boolean CheckForCriticalHit(String text)
        {
            return text.Contains(CriticalIdentifier);
        }
    }
}
