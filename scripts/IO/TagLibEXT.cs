﻿using System;
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

        /// <summary>
        /// This refreshes the file to get rid of unused bytes and saves all made changes.
        /// </summary>
        /// <param name="Item"></param>
        /// <param name="Source"></param>
        public static void Refresh(this TagLib.File Item, string Source)
        {
            //temporary file
            TagLib.File newSong2 = TagLib.File.Create(Source);

            //temporary tag that is null
            TagLib.Tag tempTag = new TagLib.Id3v2.Tag();

            //cache tags
            Item.Tag.CopyTo(tempTag, true);

            //clear all tags
            Item.RemoveTags(TagLib.TagTypes.AllTags);

            Item.Save();
            Item.Dispose();

            //rewrite the old tags to the file
            tempTag.CopyTo(newSong2.Tag, true);

            newSong2.Save();
            newSong2.Dispose();
        }
    }
}