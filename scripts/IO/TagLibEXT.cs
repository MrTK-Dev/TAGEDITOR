using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ID3_Tag_Editor.Scripts.IO
{
    public static class TagLibEXT
    {
        public static Bitmap GetCoverImage(TagLib.File newSong)
        {
            MemoryStream mStream = new MemoryStream();

            TagLib.IPicture firstPicture = newSong.Tag.Pictures.FirstOrDefault();

            if (firstPicture != null)
            {
                byte[] pData = firstPicture.Data.Data;

                mStream.Write(pData, 0, Convert.ToInt32(pData.Length));

                Bitmap CoverImage = new Bitmap(mStream, false);

                mStream.Dispose();

                return CoverImage;
            }

            return null;
        }

        /// <summary>
        /// Sets the cover image of the given song to the given Picture. Setting it to "null" should result in a deletion.
        /// </summary>
        /// <param name="Song"></param>
        /// <param name="newPicture"></param>
        public static void SetImage(TagLib.File Song, TagLib.Picture newPicture)
        {
            //some var that I do not get
            TagLib.IPicture[] pic = new TagLib.IPicture[1];
            pic[0] = newPicture;

            //overwrite cover inage
            Song.Tag.Pictures = pic;

            //TODO this should be done in the main methode
            Song.Save();
        }
    }
}
