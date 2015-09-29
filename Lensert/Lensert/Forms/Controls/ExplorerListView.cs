using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lensert.Classes.Controls
{
    class ExplorerListView : ListView
    {
           public ExplorerListView()
           {
               HandleCreated += (sender, args) => NativeHelper.SetWindowTheme(this.Handle, "explorer", null);
               DoubleBuffered = true;
           }
    }
}
