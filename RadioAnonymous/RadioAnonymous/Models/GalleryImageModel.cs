using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioAnonymous.Models
{
    public class GalleryImageModel
    {
        #region Fields

        private string imagePath;

        #endregion // Fields

        #region Properties

        public string ImagePath
        {
            get { return imagePath; }
            set { imagePath = value; }
        }

        #endregion // Properties

        #region Constructors
        public GalleryImageModel(string imagePath)
        {
            this.imagePath = imagePath;
        }
        #endregion // Constructors

        #region Methods

        #endregion // Methods

    }
}
