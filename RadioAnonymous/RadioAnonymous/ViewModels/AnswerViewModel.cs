using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RadioAnonymous.Helpers;
using RadioAnonymous.Helpers.Extentions;
using RadioAnonymous.Models;
using Windows.UI.Xaml;

namespace RadioAnonymous.ViewModels
{
    public class AnswerViewModel : BaseViewModel
    {
        #region Fields

        private string uri;

        private HashsetObservableCollection<AnswerModel> answerModels;

        private DateTime dateTime;

        #endregion // Fields

        #region Properties
        public IEnumerable<AnswerModel> AnswerModels
        {
            get
            {
                for (int i = answerModels.Count - 1; i > 24; i--)
                {
                    answerModels.RemoveAt(i);
                }
                return answerModels.Reverse().Take(25);
            }
        }

        #endregion // Properties

        #region Constructors

        public AnswerViewModel()
        {
            answerModels = new HashsetObservableCollection<AnswerModel>();
            uri = "http://anon.fm/answers.html";
            dateTime = new DateTime();

            dispatcherTimer_Tick(null, null);
            var dispatcherTimer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 12) };
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Start();
        }

        #endregion // Constructors

        #region Methods

        #region Timer

        private async void dispatcherTimer_Tick(object sender, object e)
        {
            if (await UpdateAnswers())
            {
                RaisePropertyChanged("AnswerModels");
            }
        }
        
        #endregion // Timer
        
        private async Task<bool> UpdateAnswers()
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
                            try
                            {
                                AnswerModelParser(ref str);
                            }
                            catch (Exception)
                            {
                                LogViewModel.Instance.Log(LogViewModel.LogLevel.Debug, LogViewModel.LogMassage.AnotherMassage, "Can't parse Dj answers. Document structure isn't valid.");
                            }
                            
                            return true;
                        }
                    case HttpStatusCode.NotModified:
                        return false;
                }
            }
            catch (Exception)
            {
                LogViewModel.Instance.Log(LogViewModel.LogLevel.Error, LogViewModel.LogMassage.BadInternetConnection, "Can't load Dj answers.");
            }
            
            return false;
        }

        #region AnswerParser
        private void AnswerModelParser(ref string str)
        {
            var index_P = str.LastIndexOf("</p>");

            while (index_P >= 0)
            {
                var indexP = str.LastIndexOf("<p>", index_P);
                if (indexP > 0)
                {
                    var post = str.Remove(index_P).Remove(0, indexP + 3);
                    answerModels.Add(ParsePost(ref post));
                }
                else
                {
                    throw new Exception("Document structure isn't valid");
                }

                index_P = str.LastIndexOf("</p>", indexP);
            }
        }

        private AnswerModel ParsePost(ref string post)
        {
            var listUrls = ParseUrls(ref post);
            var userPost = ParseSpan(ref post, "userpost");
            var timestamp = ParseSpan(ref post, "timestamp");
            if (userPost != null)
            {
                return new AnswerModel(ParseDjAnswer(ref post), userPost)
                    {
                        Timestamp = timestamp,
                        UserId = ParseSpan(ref post, "user_id"),
                        ListUrls = listUrls
                    };
            }

            return new AnswerModel(ParseDjAnswerPost(ref post)) { Timestamp = timestamp, ListUrls = listUrls }; 
        }
        private string ParseSpan(ref string post, string span)
        {
            span = "<span class=\"" + span + "\">";
            var indexSpan = post.IndexOf(span);
            if (indexSpan >= 0)
            {
                var index_Span = post.IndexOf("</span>", indexSpan);
                if (index_Span > 0)
                {
                    return post.Remove(index_Span).Remove(0, indexSpan + span.Length).Trim(' ', '\n');
                }
            }

            return null;
        }

        private string ParseDjAnswer(ref string post)
        {
            post = post.Replace("<br />", "<br/>");
            var indexBr = post.IndexOf("<br/>");
            return indexBr >= 0 ? post.Remove(0, indexBr + 5).Replace("<br/>", "").Trim(' ', '\n') : null;
        }

        private string ParseDjAnswerPost(ref string post)
        {
            var index_Span = post.LastIndexOf("</span>");
            return index_Span >= 0 ? post.Remove(0, index_Span + 7).Replace("<br/>", "").Trim(' ', '\n').TrimStart('-').TrimStart(' ', '\n') : null;
        }

        private HashSet<string> ParseUrls(ref string post)
        {
            var listUrls = new HashSet<string>();
            const string urlPattern = @"http(s)?://([\w+?\.\w+])+([a-zA-Z0-9\~\!\@\#\$\%\^\&amp;\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)?";

            post = Regex.Replace(post, @"<a href=""" + urlPattern + @""" target=""_blank"">[ \n]*" + urlPattern + @"[ \n]*</a>", delegate(Match match)
            {
                var url = Regex.Match(match.ToString(), urlPattern);
                return url.Success ? url.Value : "";
            });

            var matches = Regex.Matches(post, urlPattern);
           
            foreach (var match in matches)
            {
                listUrls.Add(match.ToString());
            }

            return listUrls;
        }

        #endregion // AnswerParser
   
        #endregion // Methods

    }
}
