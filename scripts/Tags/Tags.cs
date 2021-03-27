using ID3_Tag_Editor.Scripts.Extensions;
using ID3_Tag_Editor.Scripts.IO;
using ID3_Tag_Editor.Scripts.User;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ID3_Tag_Editor.Scripts.Tags
{
    public static class TagProcessing
    {
        #region Variables

        static readonly string[] remixSigns = new string[] { "Remix", "Cover", "Flip", "Rework", "Edit", "Redo", "VIP", "Refix" };

        static readonly string[] featureSigns = new string[] { "ft", "ft.", "feat", "feat." };

        #endregion

        #region Main Methods

        #region Initialization

        public static void ProcessAllSongs()
        {
            Debug.WriteLine("starting to process all songs... " + Paths.Import);

            string[] allFiles = FileSystem.GetFilesFromDirectory(Paths.Import, true);

            if (allFiles != null)
                for (int i = 0; i < allFiles.Length; i++)
                    if (!string.IsNullOrEmpty(allFiles[i]) && allFiles[i].Contains(".mp3"))
                        ProcessSong(allFiles[i]);
            //TempScripts.ProcessAMatter(allFiles[i]);
        }

        #endregion

        #region Processing

        private static void ProcessSong(string newFile)
        {
            Debug.WriteLine(newFile);
            TagLib.File newSong = TagLib.File.Create(newFile);
            /*
            //Lyrics Test
            Debug.WriteLine("(before)Test Lyrics: " + newSong.Tag.Lyrics);

            newSong.Tag.Lyrics = "";
            newSong.Tag.Lyrics = "Wad ya mine";

            Debug.WriteLine("(after)Test Lyrics: " + newSong.Tag.Lyrics);
            */
            newSong.Save();

            if (newSong.Tag.FirstPerformer != null && newSong.Tag.Title != null)
            {
                string newFileName = GetFileName(newSong.Tag.FirstPerformer, newSong.Tag.Title);

                if (string.IsNullOrEmpty(newSong.Tag.Album))
                {
                    if (IsRemix(newSong.Tag.Title))
                        newSong.Tag.Album = GetRealTitle(newSong.Tag.Title) + " (The Remixes)";

                    else
                        newSong.Tag.Album = GetRealTitle(newSong.Tag.Title) + " - Single";
                }

                //TEMP           
                newSong.Tag.Comment = " ";
                newSong.Tag.Comment = "Processed by TagEditor";
                newSong.Tag.Genres = new string[] { "Electronic" };
                newSong.Tag.Performers = new string[] { "DROELOE" };
                newSong.Tag.Album = "A Matter Of Perspective";
                newSong.Tag.AlbumArtists = new string[] { "DROELOE" };

                newSong.Refresh(newFile);

                //Manages the result folder for the processed song. This allows the user to sort the songs.
                if (Modes.exportTarget != Modes.ExportTarget.none)
                {
                    if (Modes.exportTarget == Modes.ExportTarget.ONEFOLDER)
                        FileSystem.Files.Move(FileSystem.GetFileName(newFile, false), Paths.Import, newFileName, Paths.Export);

                    else if (Modes.exportTarget == Modes.ExportTarget.FOLDERS)
                    {
                        string newSortingString;

                        switch (Modes.sortingTarget)
                        {
                            case Modes.SortingTarget.ALBUM:
                                newSortingString = newSong.Tag.Album;
                                break;

                            case Modes.SortingTarget.PERFORMER:
                                newSortingString = newSong.Tag.FirstPerformer;
                                break;

                            case Modes.SortingTarget.YEAR:
                                newSortingString = newSong.Tag.Year.ToString();
                                break;

                            case Modes.SortingTarget.GENRE:
                                newSortingString = newSong.Tag.FirstGenre;
                                break;

                            case Modes.SortingTarget.none:
                                newSortingString = "ERROR";
                                break;

                            default:
                                newSortingString = newSong.Tag.Album;
                                break;
                        }

                        FileSystem.Files.Move(
                            FileSystem.GetFileName(newFile, false),
                            Paths.Import, newFileName, 
                            Paths.Export + @"/" + newSortingString.Replace(":", " ").Replace("/", "_")
                        );
                    }
                }
            }

            if (Modes.imageCheck != Modes.ImageCheck.none)
                CheckImage(newSong);

            else
                Debug.WriteLine(newFile + " does not have the required Tags!");
        }

        private static void CheckImage(TagLib.File newSong)
        {
            Bitmap coverImage = TagLibEXT.GetCoverImage(newSong);

            //TODO add better output
            if (Modes.imageCheck == Modes.ImageCheck.SQUARE || Modes.imageCheck != Modes.ImageCheck.ALL)
                if (coverImage.Width != coverImage.Height)
                    Console.WriteLine("!= Square: {0} [{1} x {2}]", newSong.Tag.Title, coverImage.Width, coverImage.Height);

            if (Modes.imageCheck == Modes.ImageCheck.SQUARE || Modes.imageCheck != Modes.ImageCheck.ALL)
                if (coverImage.Width > Modes.imageMaxSize)
                    Console.WriteLine(Modes.imageMaxSize + "< {0} [{1} x {2}]", newSong.Tag.Title, coverImage.Width, coverImage.Height);
        }

        #endregion

        #endregion

        #region Helper Methods

        static string GetFileName(string firstPerformer, string Title)
        {
            return string.Format("{0} - {1}.mp3", ReplaceInvalidValues(firstPerformer), ReplaceInvalidValues(Title));
        }

        static bool IsRemix(string fullTitle)
        {
            if (fullTitle.Contains("["))
                return true;

            if (fullTitle.Contains("("))
            {
                string secondPart = fullTitle.Split('(')[1];

                if (secondPart.Contains(" "))
                    if (secondPart.Split(' ')[0].EqualsAny(featureSigns, true))
                        return false;

                return secondPart.ContainsAny(remixSigns);
            }

            return false;
        }

        static string ReplaceInvalidValues(string newString)
        {
            return newString.Replace(":", " ").Replace("<", "").Replace(">", "").Replace("/", "_").Replace(@"\", "_"); ;
        }

        static string GetRealTitle(string fullTitle)
        {
            if (fullTitle.Contains("("))
                return fullTitle.Split('(')[0].RemoveLastChar();

            return fullTitle;
        }

        #endregion
    }

    #region Enums

    public static class Modes
    {
        public static ExportTarget exportTarget = ExportTarget.ONEFOLDER;

        public enum ExportTarget
        {
            ONEFOLDER,
            FOLDERS,
            none
        }

        public static SortingTarget sortingTarget = SortingTarget.ALBUM;

        public enum SortingTarget
        {
            ALBUM,
            PERFORMER,
            YEAR,
            GENRE,
            none
        }

        public static ImageCheck imageCheck = ImageCheck.ALL;

        public static int imageMaxSize = 2000;

        public enum ImageCheck
        {
            ALL,
            SIZE,
            SQUARE,
            none
        }

        public static ImageProcessing imageProcessing = ImageProcessing.none;

        public enum ImageProcessing
        {
            none,
            DELETE,
            OVERWRITE
        }
    }

    #endregion
}
