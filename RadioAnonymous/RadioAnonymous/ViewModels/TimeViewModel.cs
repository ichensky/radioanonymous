using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace RadioAnonymous.ViewModels
{
    public class TimeViewModel : BaseViewModel
    {
        #region Fields

        #endregion // Fields

        #region Properties

        public string Time
        {
            get
            {
                var dateTime = DateTime.UtcNow.AddHours(4);
                return " " + dateTime.Hour.ToString("D2") + ":" + dateTime.Minute.ToString("D2");
            }
        }

        public string TimeLable
        {
            get { return "Московское время: "; }
        }
        #endregion // Properties

        #region Constructors
        public TimeViewModel()
        {
            dispatcherTimer_Tick(null, null);
            var dispatcherTimer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 60) };
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Start();
        }
        #endregion // Constructors

        #region Methods

        #region Timer

        void dispatcherTimer_Tick(object sender, object e)
        {
            RaisePropertyChanged("Time");
        }

        #endregion // Timer
        

        #endregion // Methods

    }
}
