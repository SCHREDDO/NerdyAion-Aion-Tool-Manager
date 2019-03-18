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
// Last Edited On: 18.03.2019
// Language: C#
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdyAion
{
    public class Skill
    {
        private String name;
        private long dmg;
        private bool crit;
        private List<long> dmgTicks;

        public String Name
        {
            get { return name; }
            set { name = value; }
        }
        public long Dmg
        {
            get { return dmg; }
            set { dmg = value; }
        }
        public bool Crit
        {
            get { return crit; }
            set { crit = value; }
        }
        public List<long> DmgTicks
        {
            get { return dmgTicks; }
            set { dmgTicks = value; }
        }

        public Skill(String name, long dmg, bool crit)
        {
            Name = name;
            Dmg = dmg;
            Crit = crit;
            DmgTicks = new List<long>();
        }

        public void AddDmgTick(long dmgTick)
        {
            DmgTicks.Add(dmgTick);
        }

        public long CalculateSkillDmg()
        {
            long dmg = 0;

            dmg += Dmg;

            for (int i = 0; i < DmgTicks.Count; i++)
            {
                dmg += DmgTicks[i];
            }

            return dmg;
        }
    }
}
