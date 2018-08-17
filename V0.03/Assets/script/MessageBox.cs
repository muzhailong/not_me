using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using winforms = System.Windows.Forms;


namespace my
{
    class MessageBox
    {
        public static void simpleTip(string content,string title)
        {
            winforms.MessageBox.Show(
                content, title, winforms.MessageBoxButtons.OK, 
                winforms.MessageBoxIcon.None,
                winforms.MessageBoxDefaultButton.Button1,
                winforms.MessageBoxOptions.ServiceNotification);
        }
    }
}
