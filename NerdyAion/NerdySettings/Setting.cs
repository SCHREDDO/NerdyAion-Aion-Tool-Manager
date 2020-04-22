using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdySettings
{
    public class Setting
    {
        private String name;
        private String value;
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
        public Boolean Saved
        {
            get { return saved; }
            set { saved = value; }
        }

        public Setting(String name, String value)
        {
            Name = name;
            Value = value;
            Saved = true;
        }
    }
}
