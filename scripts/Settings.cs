using ID3_Tag_Editor.scripts;
using System.Diagnostics;

namespace ID3_Tag_Editor
{
    class Settings
    {
        #region Variables

        static readonly string settingsPath = @"User/Settings";

        static bool firstRun = true;

        #endregion

        #region Paths

        public class UserPaths
        {
            public string Import { get; set; }
            public string Export { get; set; }
        }

        private static readonly UserPaths userPaths = new UserPaths();

        public static void LoadUserPaths(dynamic newUserPaths)
        {
            User.Paths.Import = newUserPaths.Import;

            User.Paths.Export = newUserPaths.Export;

            MainWindow.UI.userPathBoxes.Import.Text = newUserPaths.Import;

            MainWindow.UI.userPathBoxes.Export.Text = newUserPaths.Export;
        }

        public static void SaveUserPaths()
        {
            Debug.WriteLine("Starting to save userPaths to File...");

            userPaths.Import = User.Paths.Import;

            userPaths.Export = User.Paths.Export;

            FileSystem.SaveToJSON(settingsPath, "Paths", userPaths);

            Debug.WriteLine("...saving done!");
        }

        #endregion

        #region Main

        public static void Load()
        {
            if (firstRun)
            {
                if (!FileSystem.IsDirectory(settingsPath))
                    FileSystem.CreateDirectory(settingsPath);

                if (!FileSystem.IsFile(settingsPath, "Paths.json"))
                    SaveUserPaths();

                firstRun = false;
            }

            LoadUserPaths(FileSystem.ReadFromJSON2(settingsPath, "Paths"));
        }

        public static void Save()
        {
            SaveUserPaths();
        }

        #endregion
    }
}
