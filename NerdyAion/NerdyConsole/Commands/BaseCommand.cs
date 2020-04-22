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
// Created On: 15.04.2020
// Last Edited On: 21.04.2020
// Language: C#
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace NerdyConsole.Commands
{
    public abstract class BaseCommand
    {
        protected Dictionary<String, Type> TypeList { get; set; }
        protected Dictionary<String, Object> ObjectList { get; set; }


        public BaseCommand()
        {
            TypeList = new Dictionary<String, Type>();
            ObjectList = new Dictionary<String, Object>();

            this.Obj("consoleOutput", "NerdyConsoleOutput.NerdyConsoleOutput", this.CPatch("lib", "NerdyConsoleOutput.dll"), false);
        }

        public abstract int Initialize(Dictionary<String, String> baseSettings, Dictionary<String, String> commandSettings);

        public abstract int Execute(String command, Object[] args, Dictionary<String, String> baseSettings, Dictionary<String, String> commandSettings);

        public abstract Dictionary<String, String> CInfo();

        public abstract String CHelp();

        public abstract String CHelp(bool smallInfo);

        protected void ConsoleOut(String output)
        {
            ExMethod("consoleOutput", "Output", new object[] { output }, new Type[] { typeof(String) });
        }

        protected void Obj(String indexName, String objName, String path)
        {
            Obj(indexName, objName, path, new object[] { }, true);
        }

        protected void Obj(String indexName, String objName, object[] parameter, String path)
        {
            Obj(indexName, objName, path, parameter, true);
        }

        protected void Obj(String indexName, String objName, String path, Boolean obj)
        {
            Obj(indexName, objName, path, new object[] { }, obj);
        }

        protected void Obj(String indexName, String objName, String path, object[] parameter, Boolean obj)
        {
            TypeList.Add(indexName, Assembly.LoadFrom(path).GetType(objName));

            if (obj)
            {
                ObjectList.Add(indexName, Activator.CreateInstance(TypeList[indexName], parameter));
            }
        }

        protected object ExMethod(String indexName, String method, object[] parameter, Type[] parameterType)
        {
            Object obj = null;
            if (ObjectList.ContainsKey(indexName))
            {
                obj = ObjectList[indexName];
            }

            return TypeList[indexName].GetMethod(method, parameterType).Invoke(obj, parameter);
        }

        protected String CPatch(String path, String dll)
        {
            return CPatch(new String[] { path }, dll);
        }

        protected String CPatch(String[] path, String dll)
        {
            return $"{AppDomain.CurrentDomain.BaseDirectory}{Path.Combine(path)}{Path.DirectorySeparatorChar}{dll}";
        }
    }
}
