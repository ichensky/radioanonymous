using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RadioAnonymous.Helpers.Extentions;
using RadioAnonymous.Models;
using Windows.UI.Xaml;

namespace RadioAnonymous.ViewModels
{
    public class RadioStatusViewModel : BaseViewModel
    {
        #region Fields

        private Dictionary<Kbps, string> kbpsMountId; 

        private Kbps radioStatus;

        private RadioStatusModel radioStatusModel;

        #endregion // Fields

        #region Enums
        public enum Kbps
        {
            X192,
            X64,
            X12,
            X192Back,
            X64Back,
            X12Back,
        }
        #endregion // Enums

        #region Properties

        public RadioStatusModel RadioStatusModel
        {
            get
            { 
                return radioStatusModel;
            }
        }

        public Kbps RadioStatus
        {
            get { return radioStatus; }
            set { radioStatus = value; }
        }

        public string MediaPlayerSourceUri
        {
            get
            {
                return "http://anon.fm:8000" + kbpsMountId.FirstOrDefault(d => d.Key == radioStatus).Value;
            }
        }

        public string RadioStreamDescriptionListeners
        {
            get
            {
                var listeners = "";
                if (this.radioStatusModel.MountModels != null)
                {
                    var m = this.radioStatusModel.MountModels.FirstOrDefault(mount => mount.Id == kbpsMountId.FirstOrDefault(d => d.Key == radioStatus).Value);
                       
                    if (m != null)
                    {
                        listeners = m.Listeners + " / " + m.PeakListeners;
                    }
                }
               
                    
                return listeners;
            }
        }

        public string RadioStreamDescriptionCurrentSong
        {
            get
            {
                var currentSong = "";
                if (this.radioStatusModel.MountModels != null)
                {
                    var m = this.radioStatusModel.MountModels.FirstOrDefault(mount => mount.Id == kbpsMountId.FirstOrDefault(d => d.Key == radioStatus).Value);

                    if (m != null)
                    {
                        currentSong = String.Join("\n", m.CurrentSong.SplitInParts(80));
                    }
                }


                return currentSong;
            }
        }

        public string RadioStreamDescriptionStreamDescription
        {
            get
            {
                var streamDescription = "";
                if (this.radioStatusModel.MountModels != null)
                {
                    var m = this.radioStatusModel.MountModels.FirstOrDefault(mount => mount.Id == kbpsMountId.FirstOrDefault(d => d.Key == radioStatus).Value);

                    if (m != null)
                    {
                        streamDescription = m.StreamDescription;
                    }
                }


                return streamDescription;
            }
        }

        #endregion // Properties

        #region Constructors

        #endregion // Constructors
        public RadioStatusViewModel()
        {
            this.radioStatusModel = new RadioStatusModel();
            this.RadioStatus = Kbps.X192;
            InitialeDictionaryKbpsMountId();

            dispatcherTimer_Tick(null, null);
            var dispatcherTimer = new DispatcherTimer(){ Interval = new TimeSpan(0, 0, 30) };
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Start();
        }
        #region Methods

        private void InitialeDictionaryKbpsMountId()
        {
            kbpsMountId = new Dictionary<Kbps, string>()
                {
                    { Kbps.X192, "/radio" },
                    { Kbps.X64, "/radio-low" },
                    { Kbps.X12, "/radio.aac" },
                    { Kbps.X192Back, "/timeback" },
                    { Kbps.X64Back, "/timeback-low" },
                    { Kbps.X12Back, "/timeback.aac" },
 
                };
        }

        #region Timer
        private async void dispatcherTimer_Tick(object sender, object e)
        {
            if (await this.radioStatusModel.GetRadioStatusData())
            {
                RaisePropertyChanged("RadioStatusModel");
                RaisePropertyChanged("RadioStreamDescriptionListeners");
                RaisePropertyChanged("RadioStreamDescriptionCurrentSong");
                RaisePropertyChanged("RadioStreamDescriptionStreamDescription");
            }
        }
        #endregion // Timer

        #endregion // Methods

    }
}
