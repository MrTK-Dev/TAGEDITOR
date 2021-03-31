using ID3_Tag_Editor.Scripts.User;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ID3_Tag_Editor.Scripts.IO
{
    /// <summary>
    /// Everything related to handling images. This is about saving, caching, retrieving, etc.
    /// </summary>
    public static class Images
    {
        public static class Ressources
        {
            public static string Placeholder = GetResourceLocation(@"no_cover.jpg");
        }
        
        /// <summary>
        /// Relative folder to TEMP files.
        /// </summary>
        public static string TEMP_Folder = @"resources\TEMP";

        /// <summary>
        /// Saves a given bitmap as a ".png" file.
        /// </summary>
        /// <param name="bitMap">The given Bitmap that gets saved as a file.</param>
        /// <param name="newPath">The absolute path for the new file.</param>
        /// <param name="fileName">The name of the new file + ".png".</param>
        public static void SaveBitMapToFile(Bitmap bitMap, string newPath, string fileName)
        {
            string fullpath = newPath + @"\" + fileName + ".png";

            if (!File.Exists(fullpath))
            {
                Bitmap copy = new Bitmap(bitMap);
                copy.Save(fullpath, ImageFormat.Png);

                bitMap.Dispose();
                copy.Dispose();
            }
        }

        /// <summary>
        /// Caches a given Bitmap to a temporary file and returns the absolute path.
        /// </summary>
        /// <param name="bitMap">The given Bitmap that gets cached.</param>
        /// <param name="fileName">The name of the new file + ".png".</param>
        /// <returns>The absolute path to the new file.</returns>
        public static string CacheBitMap(Bitmap bitMap, string fileName)
        {
            string newPath = Paths.GetFullPath(TEMP_Folder);

            SaveBitMapToFile(bitMap, newPath, fileName);

            return Path.Combine(newPath, fileName + ".png");
        }

        /// <summary>
        /// This clears the temporary cache folder. (<see cref="TEMP_Folder"/>)
        /// </summary>
        public static void ClearCache() => FileSystem.ClearFolder(TEMP_Folder, true);


        /// <summary>
        /// This caches the cover of a song file and returns the BitmapImage. If the cover is null a placeholder image gets returned.
        /// </summary>
        /// <param name="newFile">The given TagLib.File.</param>
        /// <returns>The cover or a placeholder as a BitmapImage.</returns>
        public static BitmapImage GetCoverUI(TagLib.File newFile)
        {
            Bitmap bitmap = TagLibEXT.GetCoverImage(newFile);

            if (bitmap == null)
                return new BitmapImage(new Uri(@"C:\Users\Megaport\source\repos\ID3 Tag Editor\resources\no_cover.jpg"));

            return new BitmapImage(new Uri(CacheBitMap(bitmap, newFile.Tag.Title)));
        }

        public static string GetResourceLocation(string fileName)
        {
            return Paths.GetFullPath("resources", fileName);
        }
    }
}
