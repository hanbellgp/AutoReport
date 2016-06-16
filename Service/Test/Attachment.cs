using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrizzlingTest
{
    public class Attachment
    {

        public string reportClass { get; set; }
        public string attName { get; set; }
        public string attType { get; set; }

        public Attachment()
        {
        }

        public Attachment(string reportClass, string attName, string attType)
        {
            this.reportClass = reportClass;
            this.attName = attName;
            this.attType = attType;
        }

    }
}
