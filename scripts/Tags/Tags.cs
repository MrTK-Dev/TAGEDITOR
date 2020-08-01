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
    public static class Tags
    {
        static readonly string[] remixSigns = new string[] { "Remix", "Cover", "Flip", "Rework", "Edit", "Redo", "VIP", "Refix" };

        static readonly string[] featureSigns = new string[] { "ft", "ft.", "feat", "feat." };

        public static void ProcessAllSongs()
        {
            Debug.WriteLine("starting to process all songs... " + Paths.Import);

            string[] allFiles = FileSystem.GetFilesFromDirectory(Paths.Import, true);

            if (allFiles != null)
                for (int i = 0; i < allFiles.Length; i++)
                    if (!string.IsNullOrEmpty(allFiles[i]) && allFiles[i].Contains(".mp3"))
                        ProcessSong(allFiles[i]);
        }

        public static void ProcessSong(string File)
        {
            Debug.WriteLine(File);

            TagLib.File newSong = TagLib.File.Create(File);

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
            }

            else
                Debug.WriteLine(File + " does not have the required Tags!");
        }

        static string GetFileName(string firstPerformer, string Title)
        {
            return string.Format("{0} - {1}", ReplaceInvalidValues(firstPerformer), ReplaceInvalidValues(Title));
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
    }
}
