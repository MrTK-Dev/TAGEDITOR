﻿using ID3_Tag_Editor.Scripts.Tags;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;

namespace ID3_Tag_Editor.Scripts.IO
{
    public static class TagLibEXT
    {
        public static Bitmap GetCoverImage(this TagLib.File newSong)
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
        public static void SetImage(this TagLib.File Song, TagLib.Picture newPicture)
        {
            if (newPicture != null)
                if (Song.Tag.Pictures.FirstOrDefault() != newPicture)
                {
                    TagLib.IPicture[] pic = new TagLib.IPicture[1];

                    pic[0] = newPicture;

                    Song.Tag.Pictures = pic;
                }

                else {}

            else
            {
                Console.WriteLine("YOOO");

                Song.Tag.Pictures = new TagLib.IPicture[0];
            }
                
        }

        public static void SetImage(this TagLib.File Song, string fullPath)
        {
            if (fullPath == null)
            {
                Song.SetImage((TagLib.Picture)null);

                Console.WriteLine("Cover is = " + fullPath);
            }
                

            else
                Song.SetImage(new TagLib.Picture(fullPath));
        }

        public static void SetInterpret(this TagLib.File item, string newInterpret)
        {
            if (newInterpret != null)
                item.Tag.Performers = new string[] { newInterpret };
        }

        public static void SetGenre(this TagLib.File item, string newGenre)
        {
            if (newGenre != null)
                item.Tag.Genres = new string[] { newGenre };
        }

        /// <summary>
        /// This refreshes the file to get rid of unused bytes and saves all made changes.
        /// </summary>
        /// <param name="Item"></param>
        /// <param name="Source"></param>

        public static void Refresh(this TagLib.File Item, string Source)
        {
            Item.Save();
            Item.Dispose();

            TagLib.File tagFile = TagLib.File.Create(Source);
            TagLib.Tag tempTag = new TagLib.Id3v2.Tag();
            tagFile.Tag.CopyTo(tempTag, true);
            tagFile.RemoveTags(TagLib.TagTypes.AllTags);
            tagFile.Save();
            tagFile.Dispose();


            TagLib.File tagFile2 = TagLib.File.Create(Source);
            tempTag.CopyTo(tagFile2.Tag, true);
            tagFile2.Save();
            tagFile2.Dispose();
        }
    }
}
