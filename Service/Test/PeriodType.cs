using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrizzlingTest
{
    public class PeriodType
    {
        private string type;
        private string name;

        public PeriodType(string type, string name)
        {
            this.type = type;
            this.name = name;
        }

        public string Type
        {
            get { return this.type; }
        }

        public string Name
        {
            get { return this.name; }
        }

    }
}
