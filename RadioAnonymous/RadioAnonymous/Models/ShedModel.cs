using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioAnonymous.Models
{
    public class ShedModel
    {
        #region Fields

        private string timeStamp;
        
        private string djName;

        private string efirName;

        #endregion // Fields

        #region Properties

        public string TimeStamp
        {
            get { return timeStamp; }
            set { timeStamp = value; }
        }

        public string DjName
        {
            get { return djName; }
            set { djName = value; }
        }

        public string EfirName
        {
            get { return efirName; }
            set { efirName = value; }
        }

        #endregion // Properties

        #region Constructors

        public ShedModel(string timeStamp, string djName, string efirName)
        {
            this.timeStamp = timeStamp.Trim(' ', '\n');
            this.djName = " " + djName.Trim(' ', '\n');
            this.efirName = " : " + efirName.Trim(':', ' ', '\n');
        }

        #endregion // Constructors

        #region Methods

        #endregion // Methods

    }
}
