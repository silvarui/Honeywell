using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPE.Gui.PresentationModels
{
    public class AlunoNotFoundException : Exception
    {
        public AlunoNotFoundException(string msg)
            : base(msg)
        {
        }
    }
}
