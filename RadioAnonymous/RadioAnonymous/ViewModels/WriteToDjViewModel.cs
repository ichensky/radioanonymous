using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RadioAnonymous.Models;
using Windows.UI.Xaml.Media.Imaging;

namespace RadioAnonymous.ViewModels
{
    public class WriteToDjViewModel : BaseViewModel
    {
        #region Fields

        private string textToDj;

        private string captchaText;

        private WriteToDjModel writeToDj;

        #endregion // Fields

        #region Properties
        public WriteToDjModel WriteToDj
        {
            get { return writeToDj; }
            set { writeToDj = value; }
        }

        public BitmapImage CaptchaImage
        {
            get
            {
                if (this.writeToDj != null)
                {
                    if (this.writeToDj.Cid != null)
                    {
                        return new BitmapImage(new Uri("http://anon.fm/feedback/" + this.writeToDj.Cid + ".gif")); 
                    }
                }

                return null;
            }
        }
        
        #endregion // Properties

        #region Constructors
        public WriteToDjViewModel()
        {
        }
        #endregion // Constructors

        #region Methods
        public async Task<bool> GetFeedbackForm()
        {
            writeToDj = new WriteToDjModel();
            var isLoadImage = await writeToDj.GetCaptchImage();
            if (isLoadImage)
            {
                RaisePropertyChanged("CaptchaImage");
            }
            return isLoadImage;
        }

        public async Task<bool> SendMessage()
        {
            var sendMessage = await writeToDj.SendMassage();
            if (!sendMessage)
            {
                RaisePropertyChanged("CaptchaImage");
            }

            return sendMessage;
        }

        #endregion // Methods

    }
}
