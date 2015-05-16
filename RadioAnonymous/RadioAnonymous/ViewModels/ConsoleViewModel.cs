using System.Collections.ObjectModel;
using RadioAnonymous.AppBlocks.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioAnonymous.ViewModels
{
    public class ConsoleViewModel : BaseViewModel<ConsoleViewModel>, IConsole
    {
        #region Fields

        private string cInStr;

        private string cOutStr;

        private ObservableCollection<string> listLog;

        #endregion // Fields

        #region Properties

        public string CInDescription
        {
            get;
            set;
        }

        public string CInStr
        {
            set { cInStr = value; }
        }

        public ObservableCollection<string> ListLog
        {
            get
            {
                while (listLog.Count > 40)
                {
                    listLog.RemoveAt(0);
                }
                return listLog;
            }
        }

        #endregion // Properties

        #region Constructors

        #endregion // Constructors

        #region Methods
        public string CIn()
        {
            return this.cInStr;
        }

        public void COut(string str, bool c = false)
        {
            if (!c)
            {
                cOutStr = str + "\n";
            }
            else
            {
                cOutStr = CInDescription + str + "\n"; 
            }

            listLog.Add(cOutStr);
        }

        public void C()
        {
            RaisePropertyChanged(x => x.CInDescription);
        }

        public void Cls()
        {
            ListLog.Clear();
        }
        #endregion // Methods
    }
}
