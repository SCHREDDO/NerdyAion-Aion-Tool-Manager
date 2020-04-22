using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bye
{
    public class Bye
    {
        private Type consoleOutput;

        public Type ConsoleOutput
        {
            get { return consoleOutput; }
            set { consoleOutput = value; }
        }

        public Bye()
        {
            String path = AppDomain.CurrentDomain.BaseDirectory + @"lib\" + "NerdyConsoleOutput.dll";

            Assembly assembly = Assembly.LoadFrom(path);
            ConsoleOutput = assembly.GetType("NerdyConsoleOutput.NerdyConsoleOutput");
            //type.GetMethod("Output").Invoke(null, new object[] { "Test" });
            //type.GetMethod("Error").Invoke(null, new object[] { "Test" });
        }

        public int Execute(String command, Object[] args, Dictionary<String, String> settings)
        {
            int code = 0;

            if (args.Length == 0)
            {
                code = -1;
            }

            return code;
        }

        public Dictionary<String, String> CInfo()
        {
            Dictionary<String, String> info = new Dictionary<String, String>();
            info.Add("name", "Bye");
            info.Add("version", "1.0.0");
            info.Add("command", "bye");
            info.Add("i1", "Unknown argument or arguments. Try 'help bye' for help.");

            return info;
        }

        public String CHelp()
        {
            return "Closes the application.\n\n"
                    + "```console\n"
                    + "bye\n"
                    + "```\n";
        }

        public String CHelp(bool smallInfo)
        {
            String info = "closes the application";

            if (!smallInfo)
            {
                info = CHelp();
            }

            return info;
        }
    }
}
