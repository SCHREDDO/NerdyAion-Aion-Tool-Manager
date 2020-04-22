using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Help
{
    public class Help
    {
        private Type consoleOutput;

        public Type ConsoleOutput
        {
            get { return consoleOutput; }
            set { consoleOutput = value; }
        }

        public Help()
        {
            String path = AppDomain.CurrentDomain.BaseDirectory + @"lib\" + "NerdyConsoleOutput.dll";

            Assembly assembly = Assembly.LoadFrom(path);
            ConsoleOutput = assembly.GetType("NerdyConsoleOutput.NerdyConsoleOutput");
            //type.GetMethod("Output").Invoke(null, new object[] { "Test" });
            //type.GetMethod("Error").Invoke(null, new object[] { "Test" });
        }

        public int Execute(String command, Object[] args, Dictionary<String, String> commads)
        {
            int code = 0;

            if (args.Length == 0)
            {
                foreach (KeyValuePair<String, String> commad in commads)
                {
                    ConsoleOutput.GetMethod("Output").Invoke(null, new object[] { commad.Key });
                }

                ConsoleOutput.GetMethod("Output").Invoke(null, new object[] { "" });
            }
            else if (args.Length == 1)
            {
                if (commads.ContainsKey(args[0].ToString()))
                {
                    String path = null;
                    Assembly assembly = null;
                    Type type = null;
                    Object obj = null;

                    foreach (KeyValuePair<String, String> commad in commads)
                    {
                        path = AppDomain.CurrentDomain.BaseDirectory + @"bin\" + commad.Value + ".dll";
                        assembly = Assembly.LoadFrom(path);
                        type = assembly.GetType(commad.Value + "." + commad.Value);
                        obj = Activator.CreateInstance(type);

                        ConsoleOutput.GetMethod("Output").Invoke(obj, new object[] { type.GetMethod("CHelp").Invoke(obj, new object[] { commad.Key }) });
                    }

                    ConsoleOutput.GetMethod("Output").Invoke(obj, new object[] { "" });
                }
                else
                {
                    code = 2;
                }
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
            info.Add("name", "Help");
            info.Add("version", "1.0.0");
            info.Add("command", "help");
            info.Add("i1", "Unknown argument or arguments. Try 'help help' for help.");
            info.Add("i2", "Unknown command. Try 'help' to get a list of commads.");

            return info;
        }

        public String CHelp()
        {
            return "Provides information about commands.If used without parameters a lists of all commands are displayed with a briefly describion.\n\n"
                    + "```console\n"
                    + "help [command]\n"
                    + "```\n\n"
                    + "Arguments:\n"
                    + "- `command`: the command you wish to receive more information about.\n";
        }

        public String CHelp(bool smallInfo)
        {
            String info = "shows commands and information";

            if (!smallInfo)
            {
                info = CHelp();
            }

            return info;
        }
    }
}
