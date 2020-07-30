using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using Newtonsoft.Json;
using System.Threading.Tasks;
using User;

namespace ID3_Tag_Editor.scripts
{
    /// <summary>
    /// Class to create, change and delete files.
    /// </summary>
    public static class FileSystem
    {
        private static string ConvertToJSON(object Object)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string newContent = System.Text.Json.JsonSerializer.Serialize(Object, options);

            return newContent;
        }

        public static void SaveToJSON(string Path, string fileName, object Content)
        {
            File.WriteAllText(Paths.GetFullPath(Path, fileName + ".json"), ConvertToJSON(Content));
        }

        /*public static dynamic ReadFromJSON(string Path, string fileName)
        {
            return System.Text.Json.JsonSerializer.Deserialize<dynamic>(File.ReadAllText(Paths.GetFullPath(Path, fileName + ".json")));
        }*/

        public static object ReadFromJSON2(string Path, string fileName)
        {
            return JsonConvert.DeserializeObject(File.ReadAllText(Paths.GetFullPath(Path, fileName + ".json")));
        }

        public static void WriteToFile(string Path, string fileName, string Content)
        {

        }

        public static void CreateDirectory(string Path)
        {
            if (!IsDirectory(Path))
            {
                Directory.CreateDirectory(Paths.GetFullPath(Path));
            }
        }

        public static bool IsDirectory(string Path)
        {
            return Directory.Exists(Paths.GetFullPath(Path));
        }

        public static bool IsFile(string Path, string fileName)
        {

            Debug.WriteLine(Paths.GetFullPath(Path, fileName));

            return File.Exists(Paths.GetFullPath(Path, fileName));
        }
    }
}
