using ID3_Tag_Editor.scripts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ID3_Tag_Editor
{
    class Settings
    {
        public static string settingsPath = @"User/Settings";

        public static bool firstRun = true;

        #region Paths

        public class UserPaths
        {
            public string Import { get; set; }
            public string Export { get; set; }
        }

        static UserPaths userPaths = new UserPaths();

        public static void LoadUserPaths(dynamic newUserPaths)
        {
            string newImport = newUserPaths.Import;

            string newExport = newUserPaths.Export;

            User.Paths.Import = newImport;

            MainWindow.UI.userPathBoxes.Import.Text = newImport;

            Debug.WriteLine("Import: " + newImport);

            User.Paths.Export = newExport;

            MainWindow.UI.userPathBoxes.Export.Text = newExport;

            Debug.WriteLine("Export: " + newExport);
        }

        #endregion

        public static void Save()
        {
            Debug.WriteLine("Starting to save userPaths to File...");

            userPaths.Import = User.Paths.Import;

            Debug.WriteLine("Import: " + userPaths.Import);

            userPaths.Export = User.Paths.Export;

            Debug.WriteLine("Export: " + userPaths.Export);

            FileSystem.SaveToJSON(settingsPath, "Paths", userPaths);

            Debug.WriteLine("...saving done!");
        }

        public static void Load()
        {
            if (firstRun)
            {
                if (!FileSystem.IsFile(settingsPath, "Paths.json"))
                {
                    FileSystem.CreateDirectory(settingsPath);

                    Save();
                }

                firstRun = false;
            }

            LoadUserPaths(FileSystem.ReadFromJSON2(settingsPath, "Paths"));
        }
    }
}
