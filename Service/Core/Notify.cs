using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hanbell.AutoReport.Core
{
    public abstract class Notify
    {
        protected abstract void Init();

        protected abstract void Send();

        public abstract void SendInfo(Notification n);

    }

}
