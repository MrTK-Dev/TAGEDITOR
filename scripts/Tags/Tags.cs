using ID3_Tag_Editor.Scripts.Extensions;
using ID3_Tag_Editor.Scripts.IO;
using ID3_Tag_Editor.Scripts.User;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        }

        #endregion

        #region Processing

        private static void ProcessSong(string newFile)
        {
            Debug.WriteLine(newFile);

            TagLib.File newSong = TagLib.File.Create(newFile);

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

                newSong.Save();

                FileSystem.Files.RenameAndMove(Paths.Import, FileSystem.GetFileName(newFile, false), Paths.Export, newFileName);
            }

            else
                Debug.WriteLine(newFile + " does not have the required Tags!");
        }

        /*private static void ProcessImage()
        {

        }*/

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
        public static ExportTarget exportTarget = ExportTarget.ONEFILE;

        public enum ExportTarget
        {
            ONEFILE,
            FOLDERS,
            none
        }
    }

    #endregion
}
