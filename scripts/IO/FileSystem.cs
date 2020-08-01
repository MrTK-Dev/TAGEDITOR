using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using Newtonsoft.Json;
using System.Threading.Tasks;
using ID3_Tag_Editor.Scripts.User;

namespace ID3_Tag_Editor.Scripts.IO
{
    /// <summary>
    /// Class to create, change and delete files.
    /// </summary>
    public static class FileSystem
    {
        /// <summary>
        /// Serializes a given object to a json-formatted string.
        /// </summary>
        /// <param name="Object">The object that gets serialized.</param>
        /// <returns>String in json format.</returns>
        private static string ConvertToJSON(object Object)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            return System.Text.Json.JsonSerializer.Serialize(Object, options); ;
        }

        /// <summary>
        /// Saves an object to the given file.
        /// </summary>
        /// <param name="Path">Folder containing the created file.</param>
        /// <param name="fileName">Name of the created file. Fileending is automatical set to ".json".</param>
        /// <param name="Content">The object that gets saved.</param>
        public static void SaveToJSON(string Path, string fileName, object Content)
        {
            File.WriteAllText(Paths.GetFullPath(Path, fileName + ".json"), ConvertToJSON(Content));
        }

        /*public static dynamic ReadFromJSON(string Path, string fileName)
        {
            return System.Text.Json.JsonSerializer.Deserialize<dynamic>(File.ReadAllText(Paths.GetFullPath(Path, fileName + ".json")));
        }*/

        /// <summary>
        /// Returns an "dynamic" object from a read file. The given object has to be used as a dynamic object, so better cache values!
        /// </summary>
        /// <param name="Path">Folder containing the wanted file.</param>
        /// <param name="fileName">Name of the wanted file. Fileending is automatical set to ".json".</param>
        /// <returns>The "dynamic" object. Better be careful!</returns>
        public static object ReadFromJSON2(string Path, string fileName)
        {
            return JsonConvert.DeserializeObject(File.ReadAllText(Paths.GetFullPath(Path, fileName + ".json")));
        }

        /*public static void WriteToFile(string Path, string fileName, string Content)
        {

        }*/

        /// <summary>
        /// Creates a directory if it does not exist.
        /// </summary>
        /// <param name="Path">Path to the wanted folder.</param>
        public static void CreateDirectory(string Path)
        {
            if (!IsDirectory(Path))
            {
                Directory.CreateDirectory(Paths.GetFullPath(Path));
            }
        }

        public static class Files
        {
            /*enum MoveType
            {
                Move,
                Rename,
                MoveAndRename
            }

            public static void Move()
            {

            }

            public static void Rename(string oldFileName, string newFileName)
            {
                
            }*/

            public static void RenameAndMove(string oldPath, string oldFileName, string newPath, string newFileName)
            {
                File.Move(oldPath + @"/" + oldFileName, newPath + @"/" + newFileName);
            }
        }

        /// <summary>
        /// Returns an array of filenames of the given path.
        /// </summary>
        /// <param name="Path">The path with the wanted files.</param>
        /// <param name="subFolders">Wether the search considers subfolders.</param>
        /// <returns>An array of filenames.</returns>
        public static string[] GetFilesFromDirectory(string Path)
        {
            return GetFilesFromDirectory(Path, false);
        }

        public static string[] GetFilesFromDirectory(string Path, bool subFolders)
        {
            if (IsDirectory(Path, !(Path == Paths.Import)))
            {
                if (subFolders)
                    return Directory.GetFiles(Path, "*", SearchOption.AllDirectories);

                else
                    return Directory.GetFiles(Path);
            }

            else
                return null;
        }

        public static bool IsDirectory(string Path)
        {
            return IsDirectory(Path, true);
        }

        /// <summary>
        /// Checks if the given directory exists. This functions exists because of the weird relative paths.
        /// </summary>
        /// <param name="Path">Path to the wanted directory.</param>
        /// <returns>True, if the directory exists.</returns>
        public static bool IsDirectory(string Path, bool isRelative)
        {
            if (isRelative)
                return Directory.Exists(Paths.GetFullPath(Path));
            
            return Directory.Exists(Path);
        }

        /// <summary>
        /// Checks if the given file exists. This functions exists because of the weird relative paths.
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="fileName"></param>
        /// <returns>True, if the file exists.</returns>
        public static bool IsFile(string Path, string fileName)
        {
            return File.Exists(Paths.GetFullPath(Path, fileName));
        }
    }
}
