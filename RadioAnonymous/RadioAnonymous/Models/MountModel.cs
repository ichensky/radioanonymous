using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RadioAnonymous.Models
{
    /// <summary>mount</summary>
    public class MountModel
    {
        #region Fields

        /// <example>/radio</example>
        public string Id  { get; set; }

        /// <example>radio Anonymous</example>
        public string StreamTitle { get; set; }

        /// <example>LIMON</example>
        public string StreamDescription { get; set; }

        /// <example>audio/mpeg</example>
        public string ContentType { get; set; }

        /// <example>17/Mar/2013:17:23:46 +0400</example>
        public string MountStart { get; set; }

        /// <example>192</example>
        public string Bitrate { get; set; }

        /// <example>37</example>
        public string Listeners { get; set; }

        /// <example>73</example>
        public string PeakListeners { get; set; }

        /// <example>Various</example>
        public string Genre { get; set; }

        /// <example>http://www.anon.fm</example>
        public string ServerUrl { get; set; }

        /// <example>Gorillaz - On Melancholy Hill</example>
        public string CurrentSong { get; set; }

        #endregion // Fields

        #region Properties

        #endregion // Properties

        #region Constructors
        public MountModel(string id)
        {
            this.Id = id;
        }
        #endregion // Constructors

        #region Methods

        #endregion // Methods
    }
}
