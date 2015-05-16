using RadioAnonymous.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml;

namespace RadioAnonymous.Helpers
{
    public class InternetHandler
    {
        /// <summary>
        /// Property that returns the connection profile [ ie, availability of Internet ]
        /// Interface type can be [ 1,6,9,23,24,37,71,131,144 ]
        /// 1 - > Some other type of network interface.
        /// 6 - > An Ethernet network interface.
        /// 9 - > A token ring network interface.
        /// 23 -> A PPP network interface.
        /// 24 -> A software loopback network interface.
        /// 37 -> An ATM network interface.
        /// 71 -> An IEEE 802.11 wireless network interface.
        /// 131 -> A tunnel type encapsulation network interface.
        /// 144 -> An IEEE 1394 (Firewire) high performance serial bus network interface.
        /// </summary>
        public static bool IsConnectedToNetwork
        {
            get
            {
                var profile = NetworkInformation.GetInternetConnectionProfile();
                if (profile != null)
                {
                    var interfaceType = profile.NetworkAdapter.IanaInterfaceType;
                    return interfaceType == 71 || interfaceType == 6;
                }
                return false;
            }
        }

        public void CheckInternesStatus()
        {
            dispatcherTimer_Tick(null, null);
            var dispatcherTimer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 3) };
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Start();
        }


        #region Timer

        private void dispatcherTimer_Tick(object sender, object e)
        {
            if (!IsConnectedToNetwork)
            {
                LogViewModel.Instance.Log(LogViewModel.LogLevel.Error, LogViewModel.LogMassage.BadInternetConnection);
            }
            else
            {
                var s = sender as DispatcherTimer;
                if (s != null)
                {
                    s.Stop();
                }
            }
        }

        #endregion // Timer

    }
}
