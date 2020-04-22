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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdyDamageAnalyzer
{
    public class Player
    {
        private String playerName;
        private String name;

        public String Name
        {
            get
            {
                if (name == "ihr" || name == "Ihr" || name == "you" || name == "You")
                {
                    return playerName;
                }

                return name; 
            }
            set { name = value; }
        }

        public List<Skill> UsedSkills { get; set; }
        public long ActionTime { get; set; }
        public long Damge { get; set; }
        public long DotDamge { get; set; }
        public long CritDamge { get; set; }

        public Player(String name)
        {
            Name = name;
            UsedSkills = new List<Skill>();
            ActionTime = 1;
            Damge = 0;
            DotDamge = 0;
            CritDamge = 0;
        }

        public Player(String name, String playerName)
        {
            this.playerName = playerName;
            Name = name;
            UsedSkills = new List<Skill>();
            ActionTime = 1;
            Damge = 0;
            DotDamge = 0;
            CritDamge = 0;
        }

        public void AddSkill(Skill skill)
        {
            if (UsedSkills.Count > 0)
            {
                int time = (skill.ActionTime - UsedSkills[(UsedSkills.Count - 1)].ActionTime).Seconds;

                if (time < 10)
                {
                    ActionTime += time;
                }
                else
                {
                    ActionTime += 1;
                }
            }
            else
            {
                ActionTime += 1;
            }

            Damge += skill.Dmg;

            if (skill.Crit)
            {
                CritDamge += skill.Dmg;
            }

            UsedSkills.Add(skill);
        }

        public Boolean AddSkillTick(String skillname, String target, long damage, DateTime actionTime)
        {
            for (int i = UsedSkills.Count - 1; i >= 0; i--)
            {
                if (UsedSkills[i].Name == skillname && UsedSkills[i].Target == target)
                {
                    UsedSkills[i].AddDmgTick(damage, actionTime);
                    Damge += damage;
                    DotDamge += damage;

                    if (UsedSkills[i].Crit)
                    {
                        CritDamge += damage;
                    }

                    return true;
                }

                if (UsedSkills[i].ActionTime.AddSeconds(60) < actionTime)
                {
                    break;
                }
            }

            return false;
        }

        public long CalculateSkillDmg()
        {
            long dmg = 0;

            foreach (Skill item in UsedSkills)
            {
                dmg += item.CalculateSkillDmg();
            }

            return dmg;
        }

        public long GetDPS()
        {
            return GetDPS(true);
        }

        public long GetDPS(Boolean byCalculaten)
        {
            if (byCalculaten)
            {
                return CalculateSkillDmg() / ActionTime;
            }

            return Damge / ActionTime;
        }
    }
}
