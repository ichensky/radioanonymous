using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RadioAnonymous.Common;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace RadioAnonymous.Pages
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class RadioAboutPage : Page
    {
        public event EventHandler CloseRequested;

        public RadioAboutPage(int marginLeft)
        {
            this.InitializeComponent();
            var bounds = Window.Current.Bounds;
            this.RootPanel.Width = bounds.Width - marginLeft;
            this.RootPanel.Height = bounds.Height;

            ImageRadioLogo.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(@"ms-appx:///images/anon.fm/logo/radio.jpg"));

            ImageAnonimuchaev.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(@"ms-appx:///images/anon.fm/images/Anonimuchaev.jpg"));


            Window.Current.SizeChanged += (sender, args) =>
            {
                bounds = Window.Current.Bounds;
                this.RootPanel.Width = bounds.Width - marginLeft;
                this.RootPanel.Height = bounds.Height;
            };
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (CloseRequested != null)
            {
                CloseRequested(this, null);
            }
        }
        
    }
}
