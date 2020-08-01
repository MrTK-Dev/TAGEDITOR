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
            TagLib.File newSong = TagLib.File.Create(File);

            Debug.WriteLine(newSong.Tag.Title);
        }
    }
}
