using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Settings
{
    public class Settings
    {
        private Type consoleOutput;

        public Type ConsoleOutput
        {
            get { return consoleOutput; }
            set { consoleOutput = value; }
        }

        public Settings()
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

            if (args.Length == 1)
            {

            }

            return code;
        }

        public Dictionary<String, String> CInfo()
        {
            Dictionary<String, String> info = new Dictionary<String, String>();
            info.Add("name", "Settings");
            info.Add("version", "1.0.0");
            info.Add("command", "settings");
            info.Add("i1", "Unknown argument or arguments. Try 'help settings' for help.");

            return info;
        }

        public String CHelp()
        {
            return "For handelt application settings.\n\n"
                    + "```console\n"
                    + "settings <command>\n"
                    + "```\n"
                    + "Arguments:\n"
                    + "- `command`: show, edit, save, undo, reset\n\n"
                    + "Argument value (show)\n"
                    + "Shows all settings an values.\n"
                    + "```console\n"
                    + "settings show\n"
                    + "```\n\n"
                    + "Argument value (edit, e)\n"
                    + "Edit the `<setting>` with the new value `< alue>`.\n"
                    + "```console\n"
                    + "settings edit [args] <setting> <value>\n"
                    + "```\n"
                    + "Arguments:\n"
                    + "- `args`: -s for saving instantly\n"
                    + "- `setting`: which setting is to be changed\n"
                    + "- `value`: what value the setting should be set to\n\n"
                    + "Argument value (save)\n"
                    + "Saved changes from settings.\n"
                    + "```console\n"
                    + "settings save\n"
                    + "```\n\n"
                    + "Argument value (undo)\n"
                    + "Reset the last changes from settings, only if not saved.\n"
                    + "```console\n"
                    + "settings undo\n"
                    + "```\n\n"
                    + "Argument value (reset)\n"
                    + "Set the settings to default back.\n"
                    + "```console\n"
                    + "settings reset\n"
                    + "```\n";
        }

        public String CHelp(bool smallInfo)
        {
            String info = "handling settings";

            if (!smallInfo)
            {
                info = CHelp();
            }

            return info;
        }
    }
}
