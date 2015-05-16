using RadioAnonymous.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Windows.Storage;

namespace RadioAnonymous.Models
{
    public class RadioStatusModel
    {
        #region Fields

        private string uri;

        private List<MountModel> mountModels;

        #endregion // Fields

        #region Properties
        public string Uri
        {
            get { return uri; }
        }

        public List<MountModel> MountModels
        {
            get { return mountModels; }
        }

        #endregion // Properties

        #region Constructors
        public RadioStatusModel(string uri = "http://anon.fm:8000/status-raw.xsl")
        {
            this.uri = uri;

        }
        #endregion // Constructors

        #region Methods

        public async Task<bool> GetRadioStatusData()
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(new Uri(uri));
                request.Method = "GET";

                using (var response = (HttpWebResponse) await request.GetResponseAsync())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (var responseStream = response.GetResponseStream())
                        using (var readStream = new StreamReader(responseStream, Encoding.UTF8))
                        {
                            var xml = XDocument.Load(readStream).Elements();

                            mountModels = new List<MountModel>();
                            if (xml.Any())
                            {
                                foreach (var status in xml)
                                {
                                    foreach (var mount in status.Elements())
                                    {
                                        var mountModel = new MountModel(mount.Attribute("id").Value);

                                        foreach (var element in mount.Elements())
                                        {
                                            switch (element.Name.LocalName)
                                            {
                                                case "stream-title":
                                                    mountModel.StreamTitle = element.Value;
                                                    break;
                                                case "stream-description":
                                                    mountModel.StreamDescription = element.Value;
                                                    break;
                                                case "content-type":
                                                    mountModel.ContentType = element.Value;
                                                    break;
                                                case "mount-start":
                                                    mountModel.MountStart = element.Value;
                                                    break;
                                                case "bitrate":
                                                    mountModel.Bitrate = element.Value;
                                                    break;
                                                case "listeners":
                                                    mountModel.Listeners = element.Value;
                                                    break;
                                                case "peak-listeners":
                                                    mountModel.PeakListeners = element.Value;
                                                    break;
                                                case "genre":
                                                    mountModel.Genre = element.Value;
                                                    break;
                                                case "server-url":
                                                    mountModel.ServerUrl = element.Value;
                                                    break;
                                                case "current-song":
                                                    mountModel.CurrentSong = element.Value;
                                                    break;
                                            }
                                        }

                                        mountModels.Add(mountModel);
                                    }
                                }
                                return true;
                            }

                        }
                    }
                }
            }
            catch
            {
                LogViewModel.Instance.Log(LogViewModel.LogLevel.Error, LogViewModel.LogMassage.BadInternetConnection, "Can't load radiostatus.");
            }

            return false;
        }
        #endregion // Methods

        
    }
}
