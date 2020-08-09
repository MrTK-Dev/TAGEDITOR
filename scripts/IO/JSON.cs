using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static ID3_Tag_Editor.Scripts.User.Preferences;

namespace ID3_Tag_Editor.Scripts.IO
{
    /// <summary>
    /// Helper class to retrieve objects from json files.
    /// </summary>
    class JSON
    {
        public static Settings ReadSettings(string Path, string fileName)
        {
            return JsonSerializer.Deserialize<Settings>(FileSystem.ReturnFromJSON(Path, fileName));
        }
    }
}
