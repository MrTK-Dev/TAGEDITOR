using ID3_Tag_Editor.Scripts.IO;
using ID3_Tag_Editor.Scripts.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ID3_Tag_Editor.Scripts
{
    public static class TempScripts
    {
        public static void ProcessAMatter(string newFile)
        {
            TagLib.File newSong = TagLib.File.Create(newFile);

            newSong.Tag.Album = "A Matter Of Perspective";
            //newSong.Tag.Performers[0] = "DROELOE";
            //newSong.Tag.Genres[0] = "Electronic";

            newSong.Refresh(newFile);

            string newFileName = "DROELOE - " + newSong.Tag.Title;

            FileSystem.Files.Move(
                FileSystem.GetFileName(newFile, false),
                Paths.Import, newFileName,
                Paths.Export
            );
        }
    }
}
