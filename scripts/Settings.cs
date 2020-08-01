using System.Diagnostics;
using ID3_Tag_Editor.Scripts.IO;

namespace ID3_Tag_Editor.Scripts.User
{
    /// <summary>
    /// This class handles everything related to saving settings made by the user. This heavily relies on the FileSystem by saving data on files.
    /// </summary>
    class Settings
    {
        #region Variables

        /// <summary>
        /// The default path to the settings of the user.
        /// </summary>
        static readonly string settingsPath = @"User/Settings";

        /// <summary>
        /// This works as a switch if it is the first run of the application to make some preparations.
        /// </summary>
        static bool firstRun = true;

        #endregion

        #region Paths

        /// <summary>
        /// Stores paths to folders that the user choose.
        /// </summary>
        public class UserPaths
        {
            /// <summary>
            /// The folder containing the files PRE-Processing
            /// </summary>
            public string Import { get; set; }

            /// <summary>
            /// The folder containing the files POST-Processing
            /// </summary>
            public string Export { get; set; }
        }

        /// <summary>
        /// Caches the paths the user choose.
        /// </summary>
        private static readonly UserPaths userPaths = new UserPaths();

        /// <summary>
        /// Loads the user preferences from the file and apply it to the cached memory.
        /// </summary>
        /// <param name="newUserPaths">The loaded object from the json file.</param>
        public static void LoadUserPaths(dynamic newUserPaths)
        {
            Paths.Import = newUserPaths.Import;

            Paths.Export = newUserPaths.Export;

            MainWindow.UI.UserPathBoxes.Import.Text = newUserPaths.Import;

            MainWindow.UI.UserPathBoxes.Export.Text = newUserPaths.Export;
        }

        /// <summary>
        /// Saves the current selection of paths to the json file.
        /// <para>First it caches the current selection to <see cref="userPaths"/> and then it saves it to the corresponding json.</para>
        /// </summary>
        public static void SaveUserPaths()
        {
            Debug.WriteLine("Starting to save userPaths to File...");

            userPaths.Import = Paths.Import;

            userPaths.Export = Paths.Export;

            FileSystem.SaveToJSON(settingsPath, "Paths", userPaths);

            Debug.WriteLine("...saving done!");
        }

        #endregion

        #region Main

        /// <summary>
        /// Loads every setting from files in the cache and applies it to the application.
        /// <para>If it is the first run, the function checks for the folder under <see cref="settingsPath"/> and creates it, if it is not there yet.
        /// It also checks, if the savefiles already exist. If not, they will be created with the default settings.</para>
        /// <para>The function loads following modules:</para>
        /// <list type="bullet">
        /// <item>
        /// <term><see cref="LoadUserPaths(dynamic)"/></term>
        /// <description>Paths for the files.</description>
        /// </item>
        /// </list>
        /// </summary>
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

        /// <summary>
        /// Saves every setting from the cache and saves it to files.
        /// <para>The function loads following modules:</para>
        /// <list type="bullet">
        /// <item>
        /// <term><see cref="SaveUserPaths"/></term>
        /// <description>Paths for the files.</description>
        /// </item>
        /// </list>
        /// </summary>
        public static void Save()
        {
            SaveUserPaths();
        }

        #endregion
    }
}
