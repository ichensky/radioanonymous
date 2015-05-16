using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioAnonymous.AppBlocks.Console
{
    public interface IConsole
    {
        string CIn();

        void COut(string str, bool c = false);

        void C();

        void Cls();

        string CInDescription { get; set; }
    }
}
