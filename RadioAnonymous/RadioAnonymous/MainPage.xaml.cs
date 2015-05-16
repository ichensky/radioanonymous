using System.Net;
using System.Text;
using RadioAnonymous.Common;
using RadioAnonymous.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RadioAnonymous.Models;
using RadioAnonymous.Pages;
using RadioAnonymous.ViewModels;
using RadioAnonymous.Views;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media;
using Windows.Media.Playlists;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace RadioAnonymous
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : LayoutAwarePage
    {
        private RadioStatusViewModel radioStatusViewModel;
        private WriteToDjViewModel writeToDjViewModel;
        private AnswerViewModel answerViewModel;
        private ShedViewModel shedViewModel;
        private BanerViewModel banerViewModel;
        private GalleryViewModel galleryViewModel;

        private Popup popup;

        public MainPage()
        {
            this.InitializeComponent();
            
            PlayState();

            StackPanelTime.DataContext = new TimeViewModel();

            radioStatusViewModel = new RadioStatusViewModel();

            StackPanelRadioStreamDescription.DataContext = radioStatusViewModel;
            GridRadioStatus.DataContext = radioStatusViewModel;

            mediaplayer.Volume = 0.75;
            SliderMediaPlayerVolume.Value = 75;

            writeToDjViewModel = new WriteToDjViewModel();

            StackPanelWriteToDj.DataContext = writeToDjViewModel;
            StackPanelWriteToDj.Visibility = Visibility.Collapsed;
            ContentControlSendMessageErrorText.Visibility = Visibility.Collapsed;

            answerViewModel = new AnswerViewModel();
            ListViewAnswers.DataContext = answerViewModel;

            GridWebBrowser.Visibility = Visibility.Collapsed;

            shedViewModel = new ShedViewModel();
            ListViewShed.DataContext = shedViewModel;

            banerViewModel = new BanerViewModel();
            ImageName.DataContext = banerViewModel;
            ImageSpash.DataContext = banerViewModel;
            banerViewModel.ImageSplashChangedEvent += (sender, args) => StoryboardSplashUploded.Begin();
            StoryboardSplashUploded.Completed += (sender, o) =>
                { banerViewModel.SourceImageSplashPath = banerViewModel.SourceImageSplashNewPath; };
            
            WebViewAnswers.LoadCompleted += (o, args) =>
            {
                var b = new WebViewBrush { SourceName = "WebViewAnswers" };
                b.Redraw();
                RectangleWebViewAnswers.Fill = b;
                WebViewAnswers.Visibility = Visibility.Collapsed;
                ComboBoxUrls.SelectedIndex = selectedUrlIndex;
            };

            WebViewAnswers.NavigationFailed += (sender, args) => LogViewModel.Instance.Log(LogViewModel.LogLevel.Error, LogViewModel.LogMassage.AnotherMassage, "Can't navigate to the uri.");

            mediaplayer.MediaFailed += (sender, args) => { LogViewModel.Instance.Log(LogViewModel.LogLevel.Error, LogViewModel.LogMassage.AnotherMassage, "Can't get radio stream.");
                                                             StopState();
            };
            
            ContentControlLog.DataContext = LogViewModel.Instance;

            galleryViewModel = new GalleryViewModel();
            GridViewGallery.DataContext = galleryViewModel;
            

            new InternetHandler().CheckInternesStatus();

        }



        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        #region States

        private void PlayState()
        {
            mediaplayer.Stop();
            ButtonPlay.Visibility = Visibility.Visible;
            ButtonStop.Visibility = Visibility.Collapsed;
        }

        private void StopState()
        {
            mediaplayer.Source = new Uri(this.radioStatusViewModel.MediaPlayerSourceUri);
            mediaplayer.Play();
            ButtonPlay.Visibility = Visibility.Collapsed;
            ButtonStop.Visibility = Visibility.Visible;
        }

        #endregion // States
        
        private void ButtonStop_Tapped(object sender, TappedRoutedEventArgs e)
        {
            PlayState();
        }

        private void ButtonPlay_Tapped(object sender, TappedRoutedEventArgs e)
        {
            StopState();
        }

        private void Button192kbps_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.radioStatusViewModel.RadioStatus = RadioStatusViewModel.Kbps.X192;
            StopState();
        }

        private void Button64kbps_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.radioStatusViewModel.RadioStatus = RadioStatusViewModel.Kbps.X64;
            StopState();
        }

        private void Button12kbps_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.radioStatusViewModel.RadioStatus = RadioStatusViewModel.Kbps.X12;
            StopState();
        }

        private void ButtonBack192kbps_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.radioStatusViewModel.RadioStatus = RadioStatusViewModel.Kbps.X192Back;
            StopState();
        }

        private void ButtonBack64kbps_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.radioStatusViewModel.RadioStatus = RadioStatusViewModel.Kbps.X64;
            StopState();
        }

        private void ButtonBack12kbps_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.radioStatusViewModel.RadioStatus = RadioStatusViewModel.Kbps.X12Back;
            StopState();
        }

        private void SliderMediaPlayerVolume_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            mediaplayer.Volume = SliderMediaPlayerVolume.Value/100;
        }

        #region Write Message to Dj
        private async void ButtonWriteToDj_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var isLoadImage = await writeToDjViewModel.GetFeedbackForm();
            if (isLoadImage)
            {
                StackPanelWriteToDj.Visibility = Visibility.Visible;
                dispatcherTimer = new DispatcherTimer() { Interval = new TimeSpan(0, 2, 0) };
                dispatcherTimer.Tick += dispatcherMessageSendTimer_Tick;
                dispatcherTimer.Start();
            }
        }

        private DispatcherTimer dispatcherTimer;

        void dispatcherMessageSendTimer_Tick(object sender, object e)
        {
            StackPanelWriteToDj.Visibility = Visibility.Collapsed;
            ContentControlSendMessageErrorText.Visibility = Visibility.Collapsed;
            writeToDjViewModel.WriteToDj.CaptchaText = "";
            dispatcherTimer.Stop();
        }

        private async void ButtonSend_Click(object sender, RoutedEventArgs e)
        {
            writeToDjViewModel.WriteToDj.TextToDj = TextBoxTextToDj.Text;
            writeToDjViewModel.WriteToDj.CaptchaText = TextBoxCapthaText.Text;
            var sendMessage = await writeToDjViewModel.SendMessage();
            if (sendMessage)
            {
                StackPanelWriteToDj.Visibility = Visibility.Collapsed;
                writeToDjViewModel.WriteToDj.TextToDj = "";
                ContentControlSendMessageErrorText.Visibility = Visibility.Collapsed;
                dispatcherTimer.Stop();
            }
            else
            {
                ContentControlSendMessageErrorText.Visibility = Visibility.Visible;
                dispatcherTimer.Start();
            }

            writeToDjViewModel.WriteToDj.CaptchaText = "";
        }
        #endregion // Write Message to Dj

        private int selectedUrlIndex = -1;
        
        private void WebViewNavigate(string uriString)
        {
            if (GridWebBrowser.Visibility == Visibility.Collapsed)
            {
                GridWebBrowser.Visibility = Visibility.Visible;
            }

            if (WebViewAnswers.Visibility == Visibility.Collapsed)
            {
                WebViewAnswers.Visibility = Visibility.Visible;
            }

            WebViewAnswers.Navigate(new Uri(uriString));
        }


        private void GridPost_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                var element = sender as FrameworkElement;
                if (element != null)
                {
                    var answerModel = element.DataContext as AnswerModel;
                    if (answerModel != null)
                    {
                        if (answerModel.ListUrls.Count > 0)
                        {
                            selectedUrlIndex = new Random().Next(0, answerModel.ListUrls.Count);

                            WebViewNavigate(answerModel.ListUrls.ElementAt(selectedUrlIndex));

                            ComboBoxUrls.ItemsSource = answerModel.ListUrls;
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void ComboBoxUrls_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (selectedUrlIndex != ComboBoxUrls.SelectedIndex)
                {
                    selectedUrlIndex = ComboBoxUrls.SelectedIndex;
                    if (ComboBoxUrls.SelectedItem != null)
                    {
                        WebViewAnswers.Navigate(new Uri(ComboBoxUrls.SelectedItem.ToString()));
                    }
                }
            }
            catch
            {

            }
        }

        private async void ImageSpash_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (await banerViewModel.ChangeSplash())
            {
                
            }
        }

        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var x = ((MatrixTransform) (WebViewAnswers.TransformToVisual(Window.Current.Content))).Matrix.OffsetX;
            if (x < 350)
            {
                if (WebViewAnswers.Visibility == Visibility.Visible)
                {
                    WebViewAnswers.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                if (WebViewAnswers.Visibility == Visibility.Collapsed)
                {
                    WebViewAnswers.Visibility = Visibility.Visible;
                }
            }
        }

        private void ImageSpash_ImageOpened(object sender, RoutedEventArgs e)
        {
            StoryboardSplashChanged.Begin();
        }

        #region menu

        private Visibility webViewVisibility;

        private void ShowPopupPage(UIElement page)
        {
            if (popup != null)
            {
                popup.IsOpen = false;
            }

            popup = new Popup()
            {
                Child = page,
                IsOpen = true
            };

            popup.HorizontalOffset += 350;

            webViewVisibility = Visibility.Collapsed;
            WebViewAnswers.Visibility = Visibility.Collapsed;
        }

        private void ClosePopupPage()
        {
            if (popup != null)
            {
                popup.IsOpen = false;
            }
            
            WebViewAnswers.Visibility = webViewVisibility;
        }

        private void GridGalleryImageItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var page = new GalleryPage(350)
                {
                    DataContext = this.galleryViewModel,
                };

            ShowPopupPage(page);

            page.Loaded += (o, args) => page.SelecteGalleryItem(GridViewGallery.SelectedIndex);
            page.CloseRequested += (o, args) => ClosePopupPage();
        }
        
        private void ButtonGalleryOpen_Tapped(object sender, TappedRoutedEventArgs e)
        {
            GridGalleryImageItem_Tapped(null, null);
        }

        private void ButtonRadioAnon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var page = new RadioAboutPage(350)
            {
            };

            ShowPopupPage(page);

            page.CloseRequested += (o, args) => ClosePopupPage();
        }

        private void ButtonRadioAnonData_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var page = new RadioDataPage(350)
            {
            };

            ShowPopupPage(page);

            page.CloseRequested += (o, args) => ClosePopupPage();
        }

        private void ButtonRadioChat_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var page = new ChatPage(350)
            {
            };

            ShowPopupPage(page);

            page.CloseRequested += (o, args) => ClosePopupPage();
        }


        private void ButtonRadioShedAll_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var page = new ShedAllPage(350)
            {
            };

            ShowPopupPage(page);

            page.CloseRequested += (o, args) => ClosePopupPage();
        }

        private void ButtonAboutApp_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var page = new AboutAppPage(350)
            {
            };

            ShowPopupPage(page);

            page.CloseRequested += (o, args) => ClosePopupPage();
        }

        #endregion // menu

        private void ButtonRadioTextSab_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ClosePopupPage();
            WebViewNavigate(@"http://anon.fm/board/");
            ComboBoxUrls.ItemsSource = new List<string>() { "board/", };
            ComboBoxUrls.SelectedIndex = 0;
        }


    }
}
