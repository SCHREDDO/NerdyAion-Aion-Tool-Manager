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
    public class Setting
    {
        private String name;
        private String value;
        private Boolean readOnly;
        private String settingsGroup;
        private Boolean saved;

        public String Name
        {
            get { return name; }
            set { name = value; }
        }
        public String Value
        {
            get { return value; }
            set { this.value = value; }
        }
        public Boolean ReadOnly
        {
            get { return readOnly; }
            set { readOnly = value; }
        }
        public String SettingsGroup
        {
            get { return settingsGroup; }
            set { settingsGroup = value; }
        }
        public Boolean Saved
        {
            get { return saved; }
            set { saved = value; }
        }

        public Setting(String name, String value, Boolean readOnly, String settingsGroup)
        {
            Name = name;
            Value = value;
            ReadOnly = readOnly;
            SettingsGroup = settingsGroup;
            Saved = true;
        }
    }
}
