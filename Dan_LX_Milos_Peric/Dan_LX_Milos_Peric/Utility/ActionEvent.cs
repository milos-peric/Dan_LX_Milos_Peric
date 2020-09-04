using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dan_LX_Milos_Peric.Utility
{
    class ActionEvent
    {
        public delegate void ActionPerformedEventHandler(object source, ActionEventArgs args);
        public event ActionPerformedEventHandler ActionPerformed;

        public void OnActionPerformed(string logMessage)
        {
            ActionPerformed?.Invoke(this, new ActionEventArgs() { LogMessage = logMessage });
        }
    }
}
