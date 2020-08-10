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
    }
}
