using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace RadioAnonymous.Helpers
{
    public class FileHandler
    {
        /// <summary>
        /// Loads the file.
        /// </summary>
        /// <param name="fileFullName">Full name of the file. ex. "RadioStations\anon.fm\radio.m3u" </param>
        /// <returns></returns>
        public async Task<IList<string>> ReadFile(string fileFullName)
        {
            var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;

            try
            {
                var file = await folder.GetFileAsync(fileFullName);

                return await FileIO.ReadLinesAsync(file);
            }
            catch (Exception)
            {
                return null;
            }

        }

        public async Task<StorageFile> GetFile(string fileFullName)
        {
            var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;

            try
            {
                return await folder.GetFileAsync(fileFullName);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
