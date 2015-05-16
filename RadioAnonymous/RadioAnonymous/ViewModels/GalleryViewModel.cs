using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RadioAnonymous.Helpers.Extentions;
using RadioAnonymous.Models;
using Windows.Storage;

namespace RadioAnonymous.ViewModels
{
    public class GalleryViewModel : BaseViewModel<GalleryViewModel>
    {
        #region Fields

        private ObservableCollection<GalleryImageModel> images; 

        #endregion // Fields

        #region Properties

        public ObservableCollection<GalleryImageModel> Images
        {
            get
            {
                return images; 
            }
        }

        #endregion // Properties
        
        #region Constructors

        public GalleryViewModel()
        {
            images = new ObservableCollection<GalleryImageModel>();
            LoadGallery();
        }

        #endregion // Constructors

        #region Methods

        private async void LoadGallery()
        {
            foreach (var file in await GetGalleryImages())
            {
                images.Add(new GalleryImageModel("../" + file.Path.Replace(Windows.ApplicationModel.Package.Current.InstalledLocation.Path + "\\", "").Replace('\\', '/')));
            }

            images.Shuffle();
        }

        private async Task<IReadOnlyList<StorageFile>> GetGalleryImages()
        {
            try
            {
                return await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("images").AsTask().Result.GetFolderAsync("anon.fm").AsTask().Result.GetFolderAsync("gallery").AsTask().Result.GetFilesAsync();
            }
            catch
            {
                LogViewModel.Instance.Log(LogViewModel.LogLevel.Warn, LogViewModel.LogMassage.AnotherMassage, "Can't get image files from folder.");
                return null;
            }
        }

        #endregion // Methods

    }
}
