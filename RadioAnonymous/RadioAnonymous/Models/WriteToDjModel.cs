using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RadioAnonymous.ViewModels;
using Windows.UI.Xaml.Media.Imaging;
using RadioAnonymous.Helpers.Extentions;

namespace RadioAnonymous.Models
{
    public class WriteToDjModel
    {
        #region Fields

        private string cid;

        private string uri;

        private string textToDj;

        private string captchaText;

        

        #endregion // Fields

        #region Properties

        public string TextToDj
        {
            get { return textToDj; }
            set { textToDj = value; }
        }

        public string CaptchaText
        {
            get { return captchaText; }
            set { captchaText = value; }
        }

        public string Uri
        {
            get { return uri; }
        }

        public string Cid
        {
            get { return cid; }
            set { cid = value; }
        }


        #endregion // Properties

        #region Constructors
        public WriteToDjModel(string uri = "http://anon.fm/feedback/")
        {
            this.uri = uri;
        }
        #endregion // Constructors

        #region Methods
        public async Task<bool> GetCaptchImage()
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

                            var feedbackForm = readStream.ReadToEnd();

                            var match = Regex.Match(feedbackForm,
                                                    @"<input[ ]+type=""hidden""[ ]+name=""cid""[ ]+value=""([0-9]+)"">");
                            if (match.Groups.Count > 0)
                            {
                                cid = match.Groups[1].Value;
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                LogViewModel.Instance.Log(LogViewModel.LogLevel.Error, LogViewModel.LogMassage.BadInternetConnection, "Can't load captcha image.");
            }

            return false;
        }
        
        public async Task<bool> SendMassage()
        {
            try
            {
                var request = (HttpWebRequest) WebRequest.Create(new Uri(uri));
                request.Method = "POST";
                string postData = "cid=" + this.cid + "&left=" + (250 - textToDj.Length) + "&msg=" + textToDj +
                                  "&check=" + captchaText;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentType = "application/x-www-form-urlencoded";
                using (var dataStream = await request.GetRequestStreamAsync())
                    dataStream.Write(byteArray, 0, byteArray.Length);

                using (var response = (HttpWebResponse) await request.GetResponseAsync())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (var responseStream = response.GetResponseStream())
                        using (var readStream = new StreamReader(responseStream, Encoding.UTF8))
                        {
                            var feedbackForm = readStream.ReadToEnd();
                            if (feedbackForm.Contains("Неверный код подтверждения"))
                            {
                                var match = Regex.Match(feedbackForm,
                                                        @"<input[ ]+type=""hidden""[ ]+name=""cid""[ ]+value=""([0-9]+)"">");
                                if (match.Groups.Count > 0)
                                {
                                    cid = match.Groups[1].Value;
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                LogViewModel.Instance.Log(LogViewModel.LogLevel.Error, LogViewModel.LogMassage.BadInternetConnection,
                                          "Can't send massage.");
            }
            return true;
        }
        #endregion // Methods

    }
}
