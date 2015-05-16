using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RadioAnonymous.Helpers.Extentions;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.UI.Xaml;

namespace RadioAnonymous.ViewModels
{
    public class BanerViewModel : BaseViewModel<BanerViewModel>
    {
        #region Fields

        private string sourceImageLogoPath;

        private string sourceImageSplashPath;

        private string sourceImageSplashNewPath;

        private string uriSplash;

        private DateTime dateTimeSplash;

        public event EventHandler ImageSplashChangedEvent;

        #endregion // Fields

        #region Properties
        
        public string SourceImageLogoPath
        {
            get { return sourceImageLogoPath; }
        }

        public string SourceImageSplashPath
        {
            get { return sourceImageSplashPath; }
            set { sourceImageSplashPath = value;
            RaisePropertyChanged(x => x.SourceImageSplashPath);
            }
        }

        public string SourceImageSplashNewPath
        {
            get { return sourceImageSplashNewPath; }
            set { 
                sourceImageSplashNewPath = value;
                var handler = ImageSplashChangedEvent;
                if (handler != null)
                    handler(this, null);
            }
        }

        #endregion // Properties

        #region Constructors

        public BanerViewModel()
        {
            sourceImageLogoPath = String.Format(@"images/anon.fm/logos/{0}.png", new Random().Next(0, 5));
            //sourceImageSplashPath = @"images/anon.fm/splash/geomap.jpg";
            ChangeSplash();
            uriSplash = @"http://anon.fm/splash.txt";

            var dispatcherTimer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 12) };
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Start();
        }
        
        #endregion // Constructors
        
        #region Methods

        public async Task<bool> ChangeSplash()
        {
            var files = await GetSplashes();
            if (files != null && files.Count > 0)
            {
                SourceImageSplashNewPath = files[new Random().Next(0, files.Count - 1)].Path.Replace(Windows.ApplicationModel.Package.Current.InstalledLocation.Path + "\\", "").Replace('\\', '/');
                return true;
            }
                
            return false;
        }
        
        private async Task<IReadOnlyList<StorageFile>> GetSplashes()
        {
            try
            {
                return await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("images").AsTask().Result.GetFolderAsync("anon.fm").AsTask().Result.GetFolderAsync("splash").AsTask().Result.GetFilesAsync();
            }
            catch
            {
                LogViewModel.Instance.Log(LogViewModel.LogLevel.Warn, LogViewModel.LogMassage.AnotherMassage, "Can't get image files from folder.");
                return null;
            }
        }

        #region Timer

        private async void dispatcherTimer_Tick(object sender, object e)
        {
            if (await UpdateSplashPath())
            {
            }
        }


        #endregion // Timer

        private async Task<bool> UpdateSplashPath()
        {
            try
            {
                var request = new HttpClient();

                request.DefaultRequestHeaders.Add("If-Modified-Since", dateTimeSplash.IfModifiedSince());
                var responce = await request.GetAsync(uriSplash);

                switch (responce.StatusCode)
                {
                    case HttpStatusCode.OK:
                        {
                            dateTimeSplash = responce.Content.Headers.LastModified.Value.DateTime;
                            var str = await request.GetStringAsync(uriSplash);

                            if (!string.IsNullOrWhiteSpace(str))
                            {
                                SourceImageSplashNewPath = new Uri(new Uri(@"http://anon.fm/"), str).AbsoluteUri;
                            }

                            return true;
                        }
                    case HttpStatusCode.NotModified:
                        return false;
                }
            }
            catch (Exception)
            {
                LogViewModel.Instance.Log(LogViewModel.LogLevel.Error, LogViewModel.LogMassage.BadInternetConnection, "Can't load splash.");
            }
            return false;
        }

        #endregion // Methods
    }
}
