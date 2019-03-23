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

namespace NerdyAion
{
    public class Player
    {
        private String name;
        private List<Skill> usedSkills;
        private long actionTime;

        public String Name
        {
            get { return name; }
            set { name = value; }
        }
        public List<Skill> UsedSkills
        {
            get { return usedSkills; }
            set { usedSkills = value; }
        }
        public long ActionTime
        {
            get { return actionTime; }
            set { actionTime = value; }
        }

        public Player(String name)
        {
            Name = name;
            UsedSkills = new List<Skill>();
            ActionTime = 0;
        }

        public void AddSkill(Skill skill)
        {
            UsedSkills.Add(skill);
        }

        public void AddSkillTick(String skillname, String target, long damage, DateTime actionTime)
        {
            for (int i = usedSkills.Count - 1; i >= 0; i--)
            {
                if (usedSkills[i].Name == skillname && usedSkills[i].Target == target)
                {
                    usedSkills[i].DmgTicks.Add(damage);
                }

                if (usedSkills[i].ActionTime.AddSeconds(60) < actionTime)
                {
                    return;
                }
            }
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
    }
}
