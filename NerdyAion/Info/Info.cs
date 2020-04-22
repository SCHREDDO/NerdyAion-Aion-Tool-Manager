using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Info
{
    public class Info
    {
        private Type consoleOutput;
        private Type nerdyINI;
        private Object nerdyINIObj;

        public Type ConsoleOutput
        {
            get { return consoleOutput; }
            set { consoleOutput = value; }
        }
        public Type NerdyINI
        {
            get { return nerdyINI; }
            set { nerdyINI = value; }
        }
        public Object NerdyINIObj
        {
            get { return nerdyINIObj; }
            set { nerdyINIObj = value; }
        }

        public Info()
        {
            String path = null;
            Assembly assembly = null;

            path = AppDomain.CurrentDomain.BaseDirectory + @"lib\" + "NerdyConsoleOutput.dll";
            assembly = Assembly.LoadFrom(path);
            ConsoleOutput = assembly.GetType("NerdyConsoleOutput.NerdyConsoleOutput");

            path = AppDomain.CurrentDomain.BaseDirectory + @"lib\" + "NerdyConsoleOutput.dll";
            assembly = Assembly.LoadFrom(path);
            NerdyINI = assembly.GetType("NerdyConsoleOutput.NerdyConsoleOutput");
            NerdyINIObj = Activator.CreateInstance(NerdyINI, new object[] { AppDomain.CurrentDomain.BaseDirectory + "nerdy.ini" });

            //type.GetMethod("Output").Invoke(null, new object[] { "Test" });
            //type.GetMethod("Error").Invoke(null, new object[] { "Test" });
        }

        public int Execute(String command, Object[] args, Dictionary<String, String> settings)
        {
            int code = 0;

            if (args.Length == 0)
            {
                ConsoleOutput.GetMethod("Output").Invoke(null, new object[] { "###################################################################" });
                ConsoleOutput.GetMethod("Output").Invoke(null, new object[] { NerdyINI.GetMethod("GetVariable").Invoke(NerdyINIObj, new object[] { "name", "info" }) + " [" + NerdyINI.GetMethod("GetVariable").Invoke(NerdyINIObj, new object[] { "version", "info" }) + "]" });
                ConsoleOutput.GetMethod("Output").Invoke(null, new object[] { "Author: " + NerdyINI.GetMethod("GetVariable").Invoke(NerdyINIObj, new object[] { "author", "info" }) });
                ConsoleOutput.GetMethod("Output").Invoke(null, new object[] { "Repository: " + NerdyINI.GetMethod("GetVariable").Invoke(NerdyINIObj, new object[] { "repository", "info" }) });
                ConsoleOutput.GetMethod("Output").Invoke(null, new object[] { "###################################################################" });
                ConsoleOutput.GetMethod("Output").Invoke(null, new object[] { "" });
            }
            else if (args.Length == 1)
            {
                switch (args[0].ToString())
                {
                    case "-a":
                        ConsoleOutput.GetMethod("Output").Invoke(null, new object[] { NerdyINI.GetMethod("GetVariable").Invoke(NerdyINIObj, new object[] { "author", "info" }) });
                        break;
                    case "-r":
                        ConsoleOutput.GetMethod("Output").Invoke(null, new object[] { NerdyINI.GetMethod("GetVariable").Invoke(NerdyINIObj, new object[] { "repository", "info" }) });
                        break;
                    case "-v":
                        ConsoleOutput.GetMethod("Output").Invoke(null, new object[] { NerdyINI.GetMethod("GetVariable").Invoke(NerdyINIObj, new object[] { "version", "info" }) });
                        break;
                    default:
                        break;
                }
                ConsoleOutput.GetMethod("Output").Invoke(null, new object[] { "" });
            }
            else
            {
                code = 1;
            }

            return code;
        }

        public Dictionary<String, String> CInfo()
        {
            Dictionary<String, String> info = new Dictionary<String, String>();
            info.Add("name", "Info");
            info.Add("version", "1.0.0");
            info.Add("command", "info");
            info.Add("i1", "Unknown argument or arguments. Try 'help info' for help.");

            return info;
        }

        public String CHelp()
        {
            return "Displayed information obout the used application.\n\n"
                    + "```console\n"
                    + "info [args]\n"
                    + "```\n\n"
                    + "Arguments:\n"
                    + "- `args`: -a, -r, -v\n";
        }

        public String CHelp(bool smallInfo)
        {
            String info = "information about the application";

            if (!smallInfo)
            {
                info = CHelp();
            }

            return info;
        }
    }
}
