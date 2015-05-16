using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using RadioAnonymous.Helpers.Extentions;
using RadioAnonymous.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace RadioAnonymous.ViewModels
{
    public class ShedViewModel : BaseViewModel
    {
        #region Fields

        private string uri;

        private ObservableCollection<ShedModel> shedModels;

        private DateTime dateTime;

        #endregion // Fields

        #region Properties

        public IEnumerable<ShedModel> ShedModels
        {
            get { return shedModels; }
        }

        #endregion // Properties

        #region Constructors
        
        public ShedViewModel(string uri = "http://anon.fm/shed.html")
        {
            shedModels = new ObservableCollection<ShedModel>();

            this.uri = uri;
            dateTime = new DateTime();

            dispatcherTimer_Tick(null, null);
            var dispatcherTimer = new DispatcherTimer() { Interval = new TimeSpan(0, 15, 0) };
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Start();
        }
        #endregion // Constructors

        #region Methods
        #region Timer

        private async void dispatcherTimer_Tick(object sender, object e)
        {
            await UpdateShed();
        }


        #endregion // Timer

        public async Task<bool> UpdateShed()
        {
            try
            {
                var request = new HttpClient();

                request.DefaultRequestHeaders.Add("If-Modified-Since", dateTime.IfModifiedSince());
                var responce = await request.GetAsync(uri);

                switch (responce.StatusCode)
                {
                    case HttpStatusCode.OK:
                        {
                            dateTime = responce.Content.Headers.LastModified.Value.DateTime;
                            var str = await request.GetStringAsync(uri);
                            shedModels.Clear();
                            try
                            {
                                ShedModelParser(str);
                            }
                            catch (Exception)
                            {
                                LogViewModel.Instance.Log(LogViewModel.LogLevel.Debug,
                                                          LogViewModel.LogMassage.AnotherMassage,
                                                          "Can't parse shedule. Document structure isn't valid.");
                            }

                            return true;
                        }
                    case HttpStatusCode.NotModified:
                        return false;
                }
            }
            catch (Exception)
            {
                LogViewModel.Instance.Log(LogViewModel.LogLevel.Error, LogViewModel.LogMassage.BadInternetConnection,
                                          "Can't load radio shedule.");
            }
            return false;
        }
        
        private void ShedModelParser(string str)
        {
            try
            {
                const int x = 500;
                const int y = 4000;
                const string timestampText = "<span class=\"timestamp\">";
                var indexTimeSpan = str.IndexOf(timestampText, x);
                var i = 0;
                var newStr = "";
                var oldIndexTimeSpan = indexTimeSpan;
                while (indexTimeSpan < y && indexTimeSpan >= 0)
                {
                    newStr = str.Remove(indexTimeSpan).Remove(0, i);
                    ParseShedule(ref newStr);

                    i = indexTimeSpan;

                    if (i + x < str.Length)
                    {
                        oldIndexTimeSpan = indexTimeSpan;
                        indexTimeSpan = str.IndexOf(timestampText, i + x);
                    }
                    else
                    {
                        newStr = str.Remove(0, indexTimeSpan + timestampText.Length);
                        ParseShedule(ref newStr);
                        break;
                    }
                }
                
                var strToHandle = str.Remove(0, oldIndexTimeSpan + timestampText.Length);

                if (indexTimeSpan == -1)
                {
                    ParseShedule(ref strToHandle);
                }
                else
                {
                    var dispatcherTimer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 1) };
                    dispatcherTimer.Tick += (sender, o) =>
                    {
                        ShedModelParser(strToHandle);
                        var obj = sender as DispatcherTimer;
                        if (obj != null)
                        {
                            obj.Stop();
                        }
                    };
                    dispatcherTimer.Start();
                }
            }
            catch (Exception)
            {
                LogViewModel.Instance.Log(LogViewModel.LogLevel.Debug, LogViewModel.LogMassage.AnotherMassage);
            }
        }


        private void ParseShedule(ref string str)
        {
            var index_Rigth = str.IndexOf("</div></body>");
            if (index_Rigth == -1)
            {
                index_Rigth = str.IndexOf("</body>");
                if (index_Rigth == -1)
                {
                    index_Rigth = str.Length;
                }
            }

            const string timestampText = "<span class=\"timestamp\">";
            const string djText = "<span class=\"dj\">";

            var indexTimeSpan = str.IndexOf(timestampText);

            while (indexTimeSpan >= 0)
            {
                var index_TimeSpan = str.IndexOf("</span>", indexTimeSpan);

                if (index_TimeSpan >= 0)
                {
                    var indexDj = str.IndexOf(djText, index_TimeSpan);
                    if (indexDj >= 0)
                    {
                        var index_Dj = str.IndexOf("</span>", indexDj);
                        if (index_Dj >= 0)
                        {
                            var indexBr = str.IndexOf("<br>", index_Dj);
                            if (indexBr >= 0)
                            {
                                shedModels.Add(new ShedModel(str.Remove(index_TimeSpan).Remove(0, indexTimeSpan + timestampText.Length), str.Remove(index_Dj).Remove(0, indexDj + djText.Length), str.Remove(indexBr).Remove(0, index_Dj + 7)));
                            }
                            else
                            {
                                shedModels.Add(new ShedModel(str.Remove(index_TimeSpan).Remove(0, indexTimeSpan + timestampText.Length), str.Remove(index_Dj).Remove(0, indexDj + djText.Length), str.Remove(index_Rigth).Remove(0, index_Dj + 7)));
                                break;
                            }
                        }
                        else
                        {
                            LogViewModel.Instance.Log(LogViewModel.LogLevel.Debug, LogViewModel.LogMassage.AnotherMassage, "Can not parse dj.");
                        }
                    }
                    else
                    {
                        LogViewModel.Instance.Log(LogViewModel.LogLevel.Debug, LogViewModel.LogMassage.AnotherMassage, "Can not parse dj.");
                    }
                }
                else
                {
                    LogViewModel.Instance.Log(LogViewModel.LogLevel.Debug, LogViewModel.LogMassage.AnotherMassage, "Can not parse timestamp.");
                }

                indexTimeSpan = str.IndexOf(timestampText, index_TimeSpan);
            }
        }
        #endregion // Methods

    }
}
