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
using System.IO;

namespace NerdyLogReader
{
    public class NerdyLogReader
    {
        private String logFilePath;
        private FileSystemWatcher logFileSystemWather;
        private long lastChecedLogFileSize;

        public String LogFilePath
        {
            get { return logFilePath; }
            set { logFilePath = value; }
        }
        private FileSystemWatcher LogFileSystemWather
        {
            get { return logFileSystemWather; }
            set { logFileSystemWather = value; }
        }
        private long LastChecedLogFileSize
        {
            get { return lastChecedLogFileSize; }
            set { lastChecedLogFileSize = value; }
        }

        public NerdyLogReader(String logFilePath)
        {
            LogFilePath = logFilePath;

            LogFileSystemWather = new FileSystemWatcher();
            LogFileSystemWather.Path = Path.GetDirectoryName(logFilePath);
            LogFileSystemWather.Filter = Path.GetFileName(logFilePath);
            LogFileSystemWather.NotifyFilter = NotifyFilters.LastWrite;
            LogFileSystemWather.Changed += LogFileChanged;

            ResetReadLogChangesPoint();
        }

        public void Start()
        {
            LogFileSystemWather.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            LogFileSystemWather.EnableRaisingEvents = false;
        }

        private void LogFileChanged(object sender, FileSystemEventArgs e)
        {

        }

        public String ReadLogByLine()
        {
            return "";
        }

        public String ReadLogAllLines()
        {
            return "";
        }

        public byte[] ReadLogChanges()
        {
            long bufferSize = (GetLogFileSize() - LastChecedLogFileSize);
            byte[] buffer = new byte[bufferSize];

            using (BinaryReader binaryReader = new BinaryReader(new FileStream(LogFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
            {
                binaryReader.BaseStream.Seek(LastChecedLogFileSize, SeekOrigin.Begin);
                binaryReader.Read(buffer, 0, (int)bufferSize);
            }

            ResetReadLogChangesPoint();

            return buffer;
        }

        public void ResetReadLogChangesPoint()
        {
            LastChecedLogFileSize = GetLogFileSize();
        }

        private long GetLogFileSize()
        {
            using (BinaryReader binaryReader = new BinaryReader(new FileStream(LogFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
            {
                return binaryReader.BaseStream.Length;
            }
        }
    }
}
