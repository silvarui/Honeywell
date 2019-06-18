using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EPE.Controls
{
    public interface IEntityGridViewEnabledCell
    {
        void EnableCell(DataGridViewRow rowEntity, string[] columnNames);
    }
}
